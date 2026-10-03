// Online play session: lobby, controller slot assignment and the lockstep frame loop.
//
//  * The host listens on a TCP port; clients connect to it (star topology).
//  * The game's four controller slots (PlayerIndex One..Four) are shared out between
//    the PCs. Each PC gets one slot per gamepad plugged into it (or one slot for the
//    keyboard if it has no gamepad). Like on the Xbox, two pilots can share a slot.
//  * Every frame each PC samples its own controllers and sends them for frame
//    (current + InputDelay). The host combines everyone's input for a frame into a
//    bundle and broadcasts it. Every PC (host included) simulates frame N only from
//    bundle N, so all simulations stay identical.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using XnaGamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace SquadronScramble.Net
{
    public sealed class NetPeer
    {
        public int Id;
        public string Name;
        public int Controllers;
        public readonly List<int> Slots = new List<int>();
        public int PingMs = -1;
        public bool Connected = true;
        internal NetConnection Connection; // host side only; null for the host itself
    }

    public sealed class NetSession : IDisposable
    {
        private sealed class Bundle
        {
            public readonly PackedPad[] Pads = new PackedPad[NetConstants.MaxSlots];
            public readonly List<GuideEvent> Events = new List<GuideEvent>();
        }

        private sealed class PeerInput
        {
            public PackedPad[] Pads;
            public GuideEvent Event;
        }

        /// <summary>The session driving the game, once an online game has started.</summary>
        public static NetSession Current { get; private set; }

        public bool IsHost { get; }
        public int Port { get; }
        public int LocalPeerId { get; private set; } = -1;
        public int InputDelay { get; set; } = NetConstants.DefaultInputDelay;
        public List<NetPeer> Peers { get; } = new List<NetPeer>();
        public bool Started { get; private set; }
        public bool InGame { get; private set; }
        public int Seed { get; private set; }
        public Dictionary<string, byte[]> SaveFiles { get; private set; } = new Dictionary<string, byte[]>();

        /// <summary>Set when the session failed or the connection was lost.</summary>
        public string Error { get; private set; }

        /// <summary>A notice to show the players (desync, or a player leaving).</summary>
        public string Warning => warning != null && (warningPersistent || clock.ElapsedMilliseconds - warningSetAt < 8000) ? warning : null;

        private string warning;
        private bool warningPersistent;
        private long warningSetAt;

        private void SetWarning(string text, bool persistent)
        {
            if (warningPersistent)
                return; // keep showing the desync warning
            warning = text;
            warningPersistent = persistent;
            warningSetAt = clock.ElapsedMilliseconds;
        }

        /// <summary>Who the host is waiting for when the game is stalled (host only).</summary>
        public string WaitingFor { get; private set; }

        public string Status { get; private set; } = string.Empty;

        public int Frame => simFrame;

        private readonly string localName;
        private readonly PlayerIndex[] localDevices;
        private readonly int[] slotOwner = { -1, -1, -1, -1 };
        private readonly PlayerIndex?[] slotLocalPad = new PlayerIndex?[NetConstants.MaxSlots];
        private readonly Stopwatch clock = Stopwatch.StartNew();

        // host
        private TcpListener listener;
        private readonly List<NetConnection> handshaking = new List<NetConnection>();
        private int nextPeerId = 1;
        private long lastPing;
        private readonly Dictionary<int, Dictionary<int, PeerInput>> hostInputs = new Dictionary<int, Dictionary<int, PeerInput>>();
        private int nextBundleFrame;
        private readonly Dictionary<int, long> hostChecks = new Dictionary<int, long>();
        private readonly List<(int frame, int peer, long value)> clientChecks = new List<(int, int, long)>();
        private bool guideCancelQueued;

        // client
        private NetConnection hostConnection;
        private Task<TcpClient> connectTask;

        // lockstep
        private readonly Dictionary<int, Bundle> bundles = new Dictionary<int, Bundle>();
        private readonly GamePadState[] currentStates = new GamePadState[NetConstants.MaxSlots];
        private int simFrame;
        private int sendFrame;
        private GuideEvent pendingGuideEvent;

        private NetSession(bool isHost, int port, string name)
        {
            IsHost = isHost;
            Port = port;
            localName = string.IsNullOrWhiteSpace(name) ? DefaultName() : name.Trim();
            if (localName.Length > 16)
                localName = localName.Substring(0, 16);
            localDevices = FindLocalControllers();
        }

        public static string DefaultName()
        {
            string n = Environment.UserName;
            return string.IsNullOrWhiteSpace(n) ? "Player" : n;
        }

        /// <summary>Gamepads plugged into this PC, or the keyboard if there are none.</summary>
        private static PlayerIndex[] FindLocalControllers()
        {
            var pads = new List<PlayerIndex>();
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                if (XnaGamePad.GetState(i).IsConnected)
                    pads.Add(i);
            }
            if (pads.Count == 0)
                pads.Add(GamePad.KeyboardPlayerIndex ?? PlayerIndex.One);
            return pads.ToArray();
        }

        public int LocalControllerCount => localDevices.Length;

        // ------------------------------------------------------------------ setup

        public static NetSession Host(int port, string name)
        {
            var s = new NetSession(true, port, name);
            try
            {
                try
                {
                    s.listener = new TcpListener(IPAddress.IPv6Any, port);
                    s.listener.Server.DualMode = true;
                    s.listener.Start();
                }
                catch (Exception)
                {
                    s.listener = new TcpListener(IPAddress.Any, port);
                    s.listener.Start();
                }
            }
            catch (Exception ex)
            {
                s.Error = $"Could not host on port {port}: {ex.Message}";
                return s;
            }
            s.LocalPeerId = 0;
            s.Peers.Add(new NetPeer { Id = 0, Name = s.localName, Controllers = s.localDevices.Length, PingMs = 0 });
            s.AssignSlots();
            s.Status = "Waiting for players to join...";
            return s;
        }

        public static NetSession Join(string address, int port, string name)
        {
            var s = new NetSession(false, port, name);
            s.Status = $"Connecting to {address}:{port}...";
            // The parameterless TcpClient picks IPv4 or IPv6 to match the address it connects to.
            s.connectTask = ConnectAsync(new TcpClient(), address, port);
            return s;
        }

        private static async Task<TcpClient> ConnectAsync(TcpClient client, string address, int port)
        {
            var connect = client.ConnectAsync(address, port);
            if (await Task.WhenAny(connect, Task.Delay(10000)) != connect)
            {
                client.Close();
                throw new TimeoutException("Timed out.");
            }
            await connect;
            return client;
        }

        /// <summary>Splits "host", "host:port" or "[v6]:port".</summary>
        public static (string host, int port) ParseAddress(string text)
        {
            text = (text ?? string.Empty).Trim();
            int port = NetConstants.DefaultPort;
            if (text.StartsWith("["))
            {
                int end = text.IndexOf(']');
                if (end > 0)
                {
                    string rest = text.Substring(end + 1);
                    if (rest.StartsWith(":") && int.TryParse(rest.Substring(1), out int p))
                        port = p;
                    return (text.Substring(1, end - 1), port);
                }
            }
            int colon = text.LastIndexOf(':');
            if (colon > 0 && text.IndexOf(':') == colon && int.TryParse(text.Substring(colon + 1), out int p2))
                return (text.Substring(0, colon), p2);
            return (text, port);
        }

        /// <summary>This PC's LAN addresses, to tell the other players.</summary>
        public static List<string> LocalAddresses()
        {
            var result = new List<string>();
            try
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != OperationalStatus.Up || ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        continue;
                    foreach (UnicastIPAddressInformation a in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (a.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a.Address))
                            result.Add(a.Address.ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            return result.Distinct().ToList();
        }

        // ------------------------------------------------------------------ lobby

        private int FreeSlots => NetConstants.MaxSlots - Peers.Where(p => p.Connected).Sum(p => p.Slots.Count);

        private void AssignSlots()
        {
            for (int i = 0; i < slotOwner.Length; i++)
                slotOwner[i] = -1;
            int slot = 0;
            foreach (NetPeer peer in Peers.Where(p => p.Connected))
            {
                peer.Slots.Clear();
                for (int k = 0; k < peer.Controllers && slot < NetConstants.MaxSlots; k++)
                {
                    peer.Slots.Add(slot);
                    slotOwner[slot] = peer.Id;
                    slot++;
                }
            }
        }

        public bool CanStart => IsHost && !Started && Error == null && Peers.Count(p => p.Connected) >= 2;

        public void ChangeInputDelay(int delta)
        {
            if (!IsHost || Started)
                return;
            InputDelay = Math.Clamp(InputDelay + delta, 1, 15);
            BroadcastLobby();
        }

        private void BroadcastLobby()
        {
            var m = Msg.Begin(MsgType.Lobby);
            WritePeers(m.writer);
            byte[] data = Msg.End(m);
            SendToClients(data);
        }

        private void WritePeers(BinaryWriter w)
        {
            w.Write((byte)InputDelay);
            var connected = Peers.Where(p => p.Connected).ToList();
            w.Write((byte)connected.Count);
            foreach (NetPeer p in connected)
            {
                w.Write(p.Id);
                w.Write(p.Name ?? string.Empty);
                w.Write((byte)p.Controllers);
                w.Write((short)p.PingMs);
                w.Write((byte)p.Slots.Count);
                foreach (int s in p.Slots)
                    w.Write((byte)s);
            }
        }

        private void ReadPeers(BinaryReader r)
        {
            InputDelay = r.ReadByte();
            Peers.Clear();
            for (int i = 0; i < slotOwner.Length; i++)
                slotOwner[i] = -1;
            int count = r.ReadByte();
            for (int i = 0; i < count; i++)
            {
                var p = new NetPeer { Id = r.ReadInt32(), Name = r.ReadString(), Controllers = r.ReadByte(), PingMs = r.ReadInt16() };
                int slots = r.ReadByte();
                for (int k = 0; k < slots; k++)
                {
                    int s = r.ReadByte();
                    p.Slots.Add(s);
                    slotOwner[s] = p.Id;
                }
                Peers.Add(p);
            }
        }

        /// <summary>Host: start the game for everyone.</summary>
        public void StartGame()
        {
            if (!CanStart)
                return;
            Seed = Environment.TickCount ^ Guid.NewGuid().GetHashCode();
            SaveFiles = ReadSaveFiles();
            // Anyone still joining is turned away.
            foreach (NetConnection c in handshaking)
            {
                c.Send(Reject("The game has already started."));
                c.Close("Rejected.");
            }
            handshaking.Clear();

            var m = Msg.Begin(MsgType.Start);
            m.writer.Write(Seed);
            m.writer.Write(SaveFiles.Count);
            foreach (var kv in SaveFiles)
            {
                m.writer.Write(kv.Key);
                m.writer.Write(kv.Value.Length);
                m.writer.Write(kv.Value);
            }
            WritePeers(m.writer);
            SendToClients(Msg.End(m));
            Started = true;
        }

        private static Dictionary<string, byte[]> ReadSaveFiles()
        {
            var files = new Dictionary<string, byte[]>();
            try
            {
                string root = StorageDevice.RootPath;
                if (Directory.Exists(root))
                {
                    foreach (string path in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
                    {
                        var info = new FileInfo(path);
                        if (info.Length > 1024 * 1024)
                            continue;
                        files[Path.GetRelativePath(root, path).Replace('\\', '/')] = File.ReadAllBytes(path);
                    }
                }
            }
            catch (Exception)
            {
            }
            return files;
        }

        private static byte[] Reject(string reason)
        {
            var m = Msg.Begin(MsgType.Reject);
            m.writer.Write(reason);
            return Msg.End(m);
        }

        // ------------------------------------------------------------------ networking pump

        /// <summary>Process network traffic. Call once per update, in the lobby and in game.</summary>
        public void Poll()
        {
            if (IsHost)
                PollHost();
            else
                PollClient();
        }

        private void PollHost()
        {
            if (listener == null)
                return;
            try
            {
                while (listener.Pending())
                {
                    TcpClient c = listener.AcceptTcpClient();
                    var conn = new NetConnection(c);
                    if (Started)
                    {
                        conn.Send(Reject("The game has already started."));
                        conn.Close("Rejected.");
                    }
                    else
                    {
                        handshaking.Add(conn);
                    }
                }
            }
            catch (Exception)
            {
            }

            for (int i = handshaking.Count - 1; i >= 0; i--)
            {
                NetConnection conn = handshaking[i];
                while (conn.TryReceive(out byte[] data))
                {
                    if (data.Length > 0 && (MsgType)data[0] == MsgType.Hello)
                    {
                        handshaking.RemoveAt(i);
                        HandleHello(conn, data);
                        break;
                    }
                }
                if (conn.IsClosed && handshaking.Contains(conn))
                    handshaking.RemoveAt(i);
            }

            foreach (NetPeer peer in Peers.ToList())
            {
                if (peer.Connection == null || !peer.Connected)
                    continue;
                while (peer.Connection.TryReceive(out byte[] data))
                    HandleFromClient(peer, data);
                if (peer.Connection.IsClosed)
                    PeerLeft(peer, peer.Connection.CloseReason);
            }

            long now = clock.ElapsedMilliseconds;
            if (now - lastPing > 1000)
            {
                lastPing = now;
                var m = Msg.Begin(MsgType.Ping);
                m.writer.Write(now);
                SendToClients(Msg.End(m));
                if (!Started)
                    BroadcastLobby();
            }

            if (InGame)
                HostBuildBundles();
        }

        private void HandleHello(NetConnection conn, byte[] data)
        {
            var r = new BinaryReader(new MemoryStream(data, 1, data.Length - 1));
            int version = r.ReadInt32();
            string name = r.ReadString();
            int controllers = Math.Clamp((int)r.ReadByte(), 1, NetConstants.MaxSlots);
            string reject = null;
            if (version != NetConstants.ProtocolVersion)
                reject = "Different game version. Everyone needs the same build of the game.";
            else if (Started)
                reject = "The game has already started.";
            else if (FreeSlots <= 0)
                reject = "The game is full (all 4 controller slots are taken).";
            if (reject != null)
            {
                conn.Send(Reject(reject));
                conn.Close("Rejected.");
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
                name = "Player";
            if (name.Length > 16)
                name = name.Substring(0, 16);
            var peer = new NetPeer { Id = nextPeerId++, Name = name, Controllers = controllers, Connection = conn };
            Peers.Add(peer);
            AssignSlots();
            var w = Msg.Begin(MsgType.Welcome);
            w.writer.Write(peer.Id);
            conn.Send(Msg.End(w));
            BroadcastLobby();
        }

        private void HandleFromClient(NetPeer peer, byte[] data)
        {
            if (data.Length == 0)
                return;
            var r = new BinaryReader(new MemoryStream(data, 1, data.Length - 1));
            switch ((MsgType)data[0])
            {
                case MsgType.Pong:
                    peer.PingMs = (int)Math.Min(9999, clock.ElapsedMilliseconds - r.ReadInt64());
                    break;
                case MsgType.Input:
                {
                    int frame = r.ReadInt32();
                    int count = r.ReadByte();
                    var pads = new PackedPad[count];
                    for (int i = 0; i < count; i++)
                        pads[i] = PackedPad.Read(r);
                    GuideEvent ev = r.ReadBoolean() ? GuideEvent.Read(r) : null;
                    HostReceiveInput(peer.Id, frame, pads, ev);
                    break;
                }
                case MsgType.Check:
                {
                    int frame = r.ReadInt32();
                    long value = r.ReadInt64();
                    clientChecks.Add((frame, peer.Id, value));
                    CompareChecks();
                    break;
                }
                case MsgType.Bye:
                    PeerLeft(peer, "left the game");
                    break;
            }
        }

        private void PeerLeft(NetPeer peer, string reason)
        {
            if (!peer.Connected)
                return;
            peer.Connected = false;
            peer.Connection?.Close(reason);
            if (!Started)
            {
                Peers.Remove(peer);
                AssignSlots();
                BroadcastLobby();
            }
            else
            {
                SetWarning($"{peer.Name} disconnected. Their controllers are now unplugged.", false);
            }
        }

        private void SendToClients(byte[] data)
        {
            foreach (NetPeer p in Peers)
            {
                if (p.Connected && p.Connection != null)
                    p.Connection.Send(data);
            }
        }

        private void PollClient()
        {
            if (connectTask != null && connectTask.IsCompleted)
            {
                if (connectTask.Status == TaskStatus.RanToCompletion)
                {
                    hostConnection = new NetConnection(connectTask.Result);
                    var m = Msg.Begin(MsgType.Hello);
                    m.writer.Write(NetConstants.ProtocolVersion);
                    m.writer.Write(localName);
                    m.writer.Write((byte)localDevices.Length);
                    hostConnection.Send(Msg.End(m));
                    Status = "Connected. Waiting for the host to start the game...";
                }
                else
                {
                    Exception ex = connectTask.Exception?.GetBaseException();
                    Error = "Could not connect: " + (ex?.Message ?? "unknown error");
                }
                connectTask = null;
            }
            if (hostConnection == null)
                return;

            while (hostConnection.TryReceive(out byte[] data))
                HandleFromHost(data);
            if (hostConnection.IsClosed && Error == null)
                Error = "Lost connection to the host" + (string.IsNullOrEmpty(hostConnection.CloseReason) ? "." : $" ({hostConnection.CloseReason})");
        }

        private void HandleFromHost(byte[] data)
        {
            if (data.Length == 0)
                return;
            var r = new BinaryReader(new MemoryStream(data, 1, data.Length - 1));
            switch ((MsgType)data[0])
            {
                case MsgType.Welcome:
                    LocalPeerId = r.ReadInt32();
                    break;
                case MsgType.Reject:
                    Error = "The host refused to let you join: " + r.ReadString();
                    hostConnection.Close("Rejected.");
                    break;
                case MsgType.Lobby:
                    ReadPeers(r);
                    break;
                case MsgType.Ping:
                {
                    var m = Msg.Begin(MsgType.Pong);
                    m.writer.Write(r.ReadInt64());
                    hostConnection.Send(Msg.End(m));
                    break;
                }
                case MsgType.Start:
                {
                    Seed = r.ReadInt32();
                    int files = r.ReadInt32();
                    SaveFiles = new Dictionary<string, byte[]>();
                    for (int i = 0; i < files; i++)
                    {
                        string path = r.ReadString();
                        SaveFiles[path] = r.ReadBytes(r.ReadInt32());
                    }
                    ReadPeers(r);
                    Started = true;
                    break;
                }
                case MsgType.Frame:
                {
                    int frame = r.ReadInt32();
                    var b = new Bundle();
                    for (int i = 0; i < NetConstants.MaxSlots; i++)
                        b.Pads[i] = PackedPad.Read(r);
                    int events = r.ReadByte();
                    for (int i = 0; i < events; i++)
                        b.Events.Add(GuideEvent.Read(r));
                    bundles[frame] = b;
                    break;
                }
                case MsgType.Desync:
                    SetWarning("The game is out of sync with the host. Quit and start a new online game.", true);
                    break;
                case MsgType.Bye:
                    Error = "The host left the game.";
                    hostConnection.Close("Host left.");
                    break;
            }
        }

        // ------------------------------------------------------------------ lockstep

        /// <summary>Call once the session has started, before the game world is created.</summary>
        public void BeginGame()
        {
            for (int s = 0; s < NetConstants.MaxSlots; s++)
                slotLocalPad[s] = null;
            NetPeer me = Peers.FirstOrDefault(p => p.Id == LocalPeerId);
            if (me != null)
            {
                for (int k = 0; k < me.Slots.Count && k < localDevices.Length; k++)
                    slotLocalPad[me.Slots[k]] = localDevices[k];
            }
            simFrame = 0;
            sendFrame = 0;
            nextBundleFrame = 0;
            InGame = true;
            Current = this;
        }

        /// <summary>Send this PC's controller input for the frames up to (current + delay).</summary>
        public void PumpInput()
        {
            if (!InGame || Error != null)
                return;
            NetPeer me = Peers.FirstOrDefault(p => p.Id == LocalPeerId);
            int slots = me?.Slots.Count ?? 0;
            while (sendFrame <= simFrame + InputDelay)
            {
                var pads = new PackedPad[slots];
                for (int k = 0; k < slots; k++)
                {
                    PlayerIndex? local = slotLocalPad[me.Slots[k]];
                    pads[k] = local.HasValue ? PackedPad.Pack(GamePad.GetLocalState(local.Value)) : PackedPad.Disconnected;
                }
                GuideEvent ev = pendingGuideEvent;
                pendingGuideEvent = null;

                if (IsHost)
                {
                    HostReceiveInput(LocalPeerId, sendFrame, pads, ev);
                }
                else
                {
                    var m = Msg.Begin(MsgType.Input);
                    m.writer.Write(sendFrame);
                    m.writer.Write((byte)pads.Length);
                    foreach (PackedPad p in pads)
                        p.Write(m.writer);
                    m.writer.Write(ev != null);
                    ev?.Write(m.writer);
                    hostConnection?.Send(Msg.End(m));
                }
                sendFrame++;
            }
            if (IsHost)
                HostBuildBundles();
        }

        private void HostReceiveInput(int peerId, int frame, PackedPad[] pads, GuideEvent ev)
        {
            if (frame < nextBundleFrame)
                return; // too late (shouldn't happen)
            if (!hostInputs.TryGetValue(frame, out var perPeer))
                hostInputs[frame] = perPeer = new Dictionary<int, PeerInput>();
            perPeer[peerId] = new PeerInput { Pads = pads, Event = ev };
        }

        private void HostBuildBundles()
        {
            while (true)
            {
                hostInputs.TryGetValue(nextBundleFrame, out var perPeer);
                var missing = Peers.Where(p => p.Connected && (perPeer == null || !perPeer.ContainsKey(p.Id))).ToList();
                if (missing.Count > 0)
                {
                    WaitingFor = string.Join(", ", missing.Select(p => p.Name));
                    return;
                }
                WaitingFor = null;

                var b = new Bundle();
                foreach (NetPeer peer in Peers.OrderBy(p => p.Id))
                {
                    if (perPeer == null || !perPeer.TryGetValue(peer.Id, out PeerInput input))
                        continue;
                    for (int k = 0; k < peer.Slots.Count && k < input.Pads.Length; k++)
                        b.Pads[peer.Slots[k]] = input.Pads[k];
                    if (input.Event != null)
                        b.Events.Add(input.Event);
                }
                bundles[nextBundleFrame] = b;
                hostInputs.Remove(nextBundleFrame);

                var m = Msg.Begin(MsgType.Frame);
                m.writer.Write(nextBundleFrame);
                foreach (PackedPad p in b.Pads)
                    p.Write(m.writer);
                m.writer.Write((byte)b.Events.Count);
                foreach (GuideEvent e in b.Events)
                    e.Write(m.writer);
                SendToClients(Msg.End(m));
                nextBundleFrame++;
            }
        }

        /// <summary>
        /// If the input for the next frame has arrived, makes it current and returns true;
        /// the caller then runs one game update with <paramref name="frameTime"/>.
        /// </summary>
        public bool TryBeginFrame(TimeSpan step, out GameTime frameTime)
        {
            frameTime = null;
            if (!InGame || !bundles.TryGetValue(simFrame, out Bundle b))
                return false;
            for (int s = 0; s < NetConstants.MaxSlots; s++)
                currentStates[s] = b.Pads[s].Unpack();
            foreach (GuideEvent e in b.Events)
                Guide.CompleteFromNetwork(e.ToResult());
            frameTime = new GameTime(TimeSpan.FromTicks(step.Ticks * simFrame), step);
            return true;
        }

        public void EndFrame()
        {
            if (simFrame % NetConstants.CheckInterval == 0)
            {
                long value = General.RandomCallCount * 1000003L + simFrame;
                if (IsHost)
                {
                    hostChecks[simFrame] = value;
                    if (hostChecks.Count > 64)
                        hostChecks.Remove(hostChecks.Keys.Min());
                    CompareChecks();
                }
                else
                {
                    var m = Msg.Begin(MsgType.Check);
                    m.writer.Write(simFrame);
                    m.writer.Write(value);
                    hostConnection?.Send(Msg.End(m));
                }
            }
            bundles.Remove(simFrame);
            simFrame++;

            // Host: if a popup is waiting on a player who has left, cancel it for everyone.
            if (IsHost)
            {
                GuideRequestInfo info = Guide.ActiveRequestPlayer;
                if (info.Active && !guideCancelQueued)
                {
                    int owner = slotOwner[(int)info.Player];
                    NetPeer p = Peers.FirstOrDefault(x => x.Id == owner);
                    if (p == null || !p.Connected)
                    {
                        pendingGuideEvent = GuideEvent.FromResult(null);
                        guideCancelQueued = true;
                    }
                }
                else if (!info.Active)
                {
                    guideCancelQueued = false;
                }
            }
        }

        private void CompareChecks()
        {
            for (int i = clientChecks.Count - 1; i >= 0; i--)
            {
                var (frame, peer, value) = clientChecks[i];
                if (!hostChecks.TryGetValue(frame, out long mine))
                {
                    if (frame < simFrame - NetConstants.CheckInterval * 64)
                        clientChecks.RemoveAt(i);
                    continue;
                }
                clientChecks.RemoveAt(i);
                if (mine != value && !warningPersistent)
                {
                    string name = Peers.FirstOrDefault(p => p.Id == peer)?.Name ?? "a player";
                    SetWarning($"The game is out of sync with {name}. Quit and start a new online game.", true);
                    SendToClients(Msg.Simple(MsgType.Desync));
                }
            }
        }

        /// <summary>The controller state the game sees for a slot on the current frame.</summary>
        public GamePadState GetSlotState(PlayerIndex slot) => currentStates[(int)slot];

        /// <summary>The gamepad on this PC that drives a slot, if this PC owns it.</summary>
        public PlayerIndex? LocalPadForSlot(PlayerIndex slot) => slotLocalPad[(int)slot];

        public bool IsLocalSlot(PlayerIndex slot) => slotLocalPad[(int)slot].HasValue;

        public string SlotOwnerName(PlayerIndex slot)
        {
            int owner = slotOwner[(int)slot];
            return Peers.FirstOrDefault(p => p.Id == owner)?.Name ?? "another player";
        }

        /// <summary>Called by the Guide overlay on the PC that owns the popup.</summary>
        public void SubmitGuideResult(object result)
        {
            pendingGuideEvent = GuideEvent.FromResult(result);
        }

        public void Dispose()
        {
            if (Current == this)
                Current = null;
            try
            {
                byte[] bye = Msg.Simple(MsgType.Bye);
                if (IsHost)
                    SendToClients(bye);
                else
                    hostConnection?.Send(bye);
            }
            catch (Exception)
            {
            }
            foreach (NetPeer p in Peers)
                p.Connection?.Close("Session ended.");
            foreach (NetConnection c in handshaking)
                c.Close("Session ended.");
            hostConnection?.Close("Session ended.");
            try { listener?.Stop(); } catch (Exception) { }
        }
    }
}
