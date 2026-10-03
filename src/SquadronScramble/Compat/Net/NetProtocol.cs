// Wire format for online play.
//
// Online play is deterministic lockstep: every PC runs the full game simulation and
// only controller input is exchanged. Each frame's input for all four controller slots
// is gathered by the host and broadcast as a "frame bundle"; a PC simulates frame N
// only once it has bundle N, so every PC sees exactly the same input on the same frame.
using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SquadronScramble.Net
{
    internal enum MsgType : byte
    {
        Hello = 1,   // client -> host: protocol, name, controller count
        Welcome,     // host -> client: your peer id
        Reject,      // host -> client: reason
        Lobby,       // host -> all: peer list and slot assignment
        Ping,        // host -> client
        Pong,        // client -> host
        Start,       // host -> all: seed, input delay, save files, final peer list
        Input,       // client -> host: input for one frame for the client's slots
        Frame,       // host -> all: input bundle for one frame
        Check,       // client -> host: desync check value for a frame
        Desync,      // host -> all: the simulations diverged
        Bye          // either way: leaving
    }

    public static class NetConstants
    {
        /// <summary>Bump when the protocol or anything affecting simulation changes.</summary>
        public const int ProtocolVersion = 1;

        public const int DefaultPort = 24642;

        public const int MaxSlots = 4;

        public const int DefaultInputDelay = 4;

        public const int CheckInterval = 60;
    }

    /// <summary>A controller state quantized to a few bytes; every PC simulates from the decoded value.</summary>
    internal struct PackedPad : IEquatable<PackedPad>
    {
        public bool Connected;
        public uint Buttons;
        public sbyte LX, LY, RX, RY;
        public byte LT, RT;

        private static readonly Buttons[] AllButtons = (Buttons[])Enum.GetValues(typeof(Buttons));

        public static readonly PackedPad Disconnected = default;

        public static PackedPad Pack(GamePadState s)
        {
            var p = new PackedPad { Connected = s.IsConnected };
            if (!s.IsConnected)
                return p;
            foreach (Buttons b in AllButtons)
            {
                if (s.IsButtonDown(b))
                    p.Buttons |= (uint)b;
            }
            p.LX = Quantize(s.ThumbSticks.Left.X);
            p.LY = Quantize(s.ThumbSticks.Left.Y);
            p.RX = Quantize(s.ThumbSticks.Right.X);
            p.RY = Quantize(s.ThumbSticks.Right.Y);
            p.LT = (byte)Math.Round(MathHelper.Clamp(s.Triggers.Left, 0f, 1f) * 255f);
            p.RT = (byte)Math.Round(MathHelper.Clamp(s.Triggers.Right, 0f, 1f) * 255f);
            return p;
        }

        private static sbyte Quantize(float v) => (sbyte)Math.Round(MathHelper.Clamp(v, -1f, 1f) * 127f);

        public GamePadState Unpack()
        {
            if (!Connected)
                return default;
            var b = (Buttons)Buttons;
            ButtonState S(Buttons x) => (b & x) != 0 ? ButtonState.Pressed : ButtonState.Released;
            return new GamePadState(
                new GamePadThumbSticks(new Vector2(LX / 127f, LY / 127f), new Vector2(RX / 127f, RY / 127f)),
                new GamePadTriggers(LT / 255f, RT / 255f),
                new GamePadButtons(b),
                new GamePadDPad(S(Microsoft.Xna.Framework.Input.Buttons.DPadUp), S(Microsoft.Xna.Framework.Input.Buttons.DPadDown),
                    S(Microsoft.Xna.Framework.Input.Buttons.DPadLeft), S(Microsoft.Xna.Framework.Input.Buttons.DPadRight)));
        }

        public void Write(BinaryWriter w)
        {
            w.Write(Connected);
            if (!Connected)
                return;
            w.Write(Buttons);
            w.Write(LX); w.Write(LY); w.Write(RX); w.Write(RY);
            w.Write(LT); w.Write(RT);
        }

        public static PackedPad Read(BinaryReader r)
        {
            var p = new PackedPad { Connected = r.ReadBoolean() };
            if (!p.Connected)
                return p;
            p.Buttons = r.ReadUInt32();
            p.LX = r.ReadSByte(); p.LY = r.ReadSByte(); p.RX = r.ReadSByte(); p.RY = r.ReadSByte();
            p.LT = r.ReadByte(); p.RT = r.ReadByte();
            return p;
        }

        public bool Equals(PackedPad o) => Connected == o.Connected && Buttons == o.Buttons && LX == o.LX && LY == o.LY && RX == o.RX && RY == o.RY && LT == o.LT && RT == o.RT;
    }

    /// <summary>
    /// The result of a Guide popup (on-screen keyboard / message box). Only the PC that owns the
    /// controller interacts with it; the result is delivered to every PC on the same frame.
    /// </summary>
    internal sealed class GuideEvent
    {
        public byte Kind; // 0 = null result, 1 = string, 2 = int
        public string Text;
        public int Value;

        public object ToResult() => Kind switch
        {
            1 => Text,
            2 => (object)(int?)Value,
            _ => null
        };

        public static GuideEvent FromResult(object result) => result switch
        {
            string s => new GuideEvent { Kind = 1, Text = s },
            int i => new GuideEvent { Kind = 2, Value = i },
            _ => new GuideEvent { Kind = 0 }
        };

        public void Write(BinaryWriter w)
        {
            w.Write(Kind);
            if (Kind == 1) w.Write(Text ?? string.Empty);
            if (Kind == 2) w.Write(Value);
        }

        public static GuideEvent Read(BinaryReader r)
        {
            var e = new GuideEvent { Kind = r.ReadByte() };
            if (e.Kind == 1) e.Text = r.ReadString();
            if (e.Kind == 2) e.Value = r.ReadInt32();
            return e;
        }
    }

    internal static class Msg
    {
        public static (BinaryWriter writer, MemoryStream stream) Begin(MsgType type)
        {
            var ms = new MemoryStream();
            var w = new BinaryWriter(ms, Encoding.UTF8);
            w.Write((byte)type);
            return (w, ms);
        }

        public static byte[] End((BinaryWriter writer, MemoryStream stream) m)
        {
            m.writer.Flush();
            return m.stream.ToArray();
        }

        public static byte[] Simple(MsgType type) => new[] { (byte)type };
    }
}
