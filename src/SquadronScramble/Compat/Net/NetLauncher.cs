// Start-up menu for the PC port: local game, host an online game, or join one.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using XnaGamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace SquadronScramble.Net
{
    public sealed class NetLauncherOptions
    {
        public bool Local;
        public bool Host;
        public string Join;
        public int Port = NetConstants.DefaultPort;
        public string Name;
        public int? InputDelay;

        public static NetLauncherOptions Parse(string[] args)
        {
            var o = new NetLauncherOptions();
            if (args == null)
                return o;
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i].ToLowerInvariant();
                string next = i + 1 < args.Length && !args[i + 1].StartsWith("-") ? args[i + 1] : null;
                switch (a)
                {
                    case "--local":
                        o.Local = true;
                        break;
                    case "--host":
                        o.Host = true;
                        if (next != null && int.TryParse(next, out int port))
                        {
                            o.Port = port;
                            i++;
                        }
                        break;
                    case "--join":
                        if (next != null)
                        {
                            o.Join = next;
                            i++;
                        }
                        break;
                    case "--port":
                        if (next != null && int.TryParse(next, out int p))
                        {
                            o.Port = p;
                            i++;
                        }
                        break;
                    case "--name":
                        if (next != null)
                        {
                            o.Name = next;
                            i++;
                        }
                        break;
                    case "--delay":
                        if (next != null && int.TryParse(next, out int d))
                        {
                            o.InputDelay = d;
                            i++;
                        }
                        break;
                }
            }
            return o;
        }
    }

    public sealed class NetLauncher
    {
        public enum Outcome
        {
            None,
            Local,
            Online,
            Quit
        }

        private enum Screen
        {
            Menu,
            Lobby,
            Message
        }

        private static readonly string[] MenuItems = { "LOCAL GAME", "HOST ONLINE GAME", "JOIN ONLINE GAME", "QUIT" };

        private readonly NetLauncherOptions options;
        private readonly SpriteFont font;
        private readonly SpriteFont titleFont;
        private readonly Texture2D background;
        private readonly Texture2D pixel;
        private Screen screen = Screen.Menu;
        private int selected;
        private string message;
        private IAsyncResult joinPrompt;
        private List<string> addresses = new List<string>();
        private KeyboardState prevKeys;
        private readonly GamePadState[] prevPads = new GamePadState[4];
        private bool firstUpdate = true;

        public Outcome Result { get; private set; }

        public NetSession Session { get; private set; }

        public NetLauncher(NetLauncherOptions options, SpriteFont font, SpriteFont titleFont, Texture2D background, Texture2D pixel)
        {
            this.options = options;
            this.font = font;
            this.titleFont = titleFont;
            this.background = background;
            this.pixel = pixel;
            if (options.Local)
                Result = Outcome.Local;
            else if (options.Host)
                StartHosting();
            else if (!string.IsNullOrEmpty(options.Join))
                StartJoining(options.Join);
        }

        private static string LastAddressFile => Path.Combine(StorageDevice.RootPath, "last-join-address.txt");

        private void StartHosting()
        {
            Session = NetSession.Host(options.Port, options.Name);
            if (options.InputDelay.HasValue)
                Session.InputDelay = Math.Clamp(options.InputDelay.Value, 1, 15);
            if (Session.Error != null)
            {
                ShowMessage(Session.Error);
                Session.Dispose();
                Session = null;
                return;
            }
            addresses = NetSession.LocalAddresses();
            screen = Screen.Lobby;
        }

        private void StartJoining(string address)
        {
            var (host, port) = NetSession.ParseAddress(address);
            if (string.IsNullOrWhiteSpace(host))
                return;
            try
            {
                Directory.CreateDirectory(StorageDevice.RootPath);
                File.WriteAllText(LastAddressFile, address.Trim());
            }
            catch (Exception)
            {
            }
            Session = NetSession.Join(host, port, options.Name);
            screen = Screen.Lobby;
        }

        private void ShowMessage(string text)
        {
            message = text;
            screen = Screen.Message;
        }

        private void LeaveSession()
        {
            Session?.Dispose();
            Session = null;
            screen = Screen.Menu;
        }

        public void Update()
        {
            KeyboardState keys = Keyboard.GetState();
            var pads = new GamePadState[4];
            for (int i = 0; i < 4; i++)
                pads[i] = XnaGamePad.GetState((PlayerIndex)i);
            if (firstUpdate)
            {
                // Don't react to keys that were already down when the menu appeared.
                prevKeys = keys;
                Array.Copy(pads, prevPads, 4);
                firstUpdate = false;
            }

            bool up = Pressed(keys, pads, Keys.Up, Keys.W, Buttons.DPadUp, Buttons.LeftThumbstickUp);
            bool down = Pressed(keys, pads, Keys.Down, Keys.S, Buttons.DPadDown, Buttons.LeftThumbstickDown);
            bool left = Pressed(keys, pads, Keys.Left, Keys.A, Buttons.DPadLeft, Buttons.LeftThumbstickLeft);
            bool right = Pressed(keys, pads, Keys.Right, Keys.D, Buttons.DPadRight, Buttons.LeftThumbstickRight);
            bool accept = Pressed(keys, pads, Keys.Enter, Keys.Space, Buttons.A, Buttons.Start);
            bool back = Pressed(keys, pads, Keys.Escape, Keys.Back, Buttons.B, Buttons.Back);
            if (keys.IsKeyDown(Keys.LeftAlt) || keys.IsKeyDown(Keys.RightAlt))
                accept = false; // Alt+Enter toggles fullscreen

            if (joinPrompt != null)
            {
                // The on-screen keyboard (Guide overlay) is open asking for the address.
                if (joinPrompt.IsCompleted)
                {
                    string address = Guide.EndShowKeyboardInput(joinPrompt);
                    joinPrompt = null;
                    if (!string.IsNullOrWhiteSpace(address))
                        StartJoining(address);
                }
            }
            else if (!Guide.IsInputBlocked)
            {
                switch (screen)
                {
                    case Screen.Menu:
                        if (up) selected = (selected + MenuItems.Length - 1) % MenuItems.Length;
                        if (down) selected = (selected + 1) % MenuItems.Length;
                        if (accept)
                        {
                            switch (selected)
                            {
                                case 0: Result = Outcome.Local; break;
                                case 1: StartHosting(); break;
                                case 2: AskForAddress(); break;
                                case 3: Result = Outcome.Quit; break;
                            }
                        }
                        else if (back)
                        {
                            Result = Outcome.Quit;
                        }
                        break;

                    case Screen.Lobby:
                        if (Session != null && Session.IsHost)
                        {
                            if (left) Session.ChangeInputDelay(-1);
                            if (right) Session.ChangeInputDelay(1);
                            if (accept && Session.CanStart)
                                Session.StartGame();
                        }
                        if (back)
                            LeaveSession();
                        break;

                    case Screen.Message:
                        if (accept || back)
                            screen = Screen.Menu;
                        break;
                }
            }

            if (Session != null)
            {
                Session.Poll();
                if (Session.Error != null)
                {
                    string error = Session.Error;
                    LeaveSession();
                    ShowMessage(error);
                }
                else if (Session.Started)
                {
                    Result = Outcome.Online;
                }
            }

            prevKeys = keys;
            Array.Copy(pads, prevPads, 4);
        }

        private void AskForAddress()
        {
            string last = string.Empty;
            try
            {
                if (File.Exists(LastAddressFile))
                    last = File.ReadAllText(LastAddressFile).Trim();
            }
            catch (Exception)
            {
            }
            joinPrompt = Guide.BeginShowKeyboardInput(PlayerIndex.One, "Join Online Game",
                $"Enter the host's IP address (add :port if the host isn't using port {NetConstants.DefaultPort}).", last, null, null);
        }

        private bool Pressed(KeyboardState keys, GamePadState[] pads, Keys k1, Keys k2, Buttons b1, Buttons b2)
        {
            if ((keys.IsKeyDown(k1) && !prevKeys.IsKeyDown(k1)) || (keys.IsKeyDown(k2) && !prevKeys.IsKeyDown(k2)))
                return true;
            for (int i = 0; i < 4; i++)
            {
                if (!pads[i].IsConnected)
                    continue;
                if ((pads[i].IsButtonDown(b1) && !prevPads[i].IsButtonDown(b1)) || (pads[i].IsButtonDown(b2) && !prevPads[i].IsButtonDown(b2)))
                    return true;
            }
            return false;
        }

        // ------------------------------------------------------------------ drawing

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            sb.Begin();
            if (background != null)
                sb.Draw(background, new Rectangle(0, 0, Dogfight.VirtualWidth, Dogfight.VirtualHeight), Color.White);
            sb.Draw(pixel, new Rectangle(0, 0, Dogfight.VirtualWidth, Dogfight.VirtualHeight), Color.Black * 0.45f);

            DrawCentered(titleFont, "SQUADRON SCRAMBLE", 70, new Color(230, 30, 30), 1f, true);

            switch (screen)
            {
                case Screen.Menu:
                    DrawMenu();
                    break;
                case Screen.Lobby:
                    DrawLobby();
                    break;
                case Screen.Message:
                    DrawPanel(new Rectangle(240, 250, 800, 260));
                    DrawWrapped(message ?? string.Empty, new Vector2(280, 290), 720, Color.White);
                    DrawCentered(font, "[Enter] OK", 470, Color.LightGray, 1f, false);
                    break;
            }
            sb.End();

            void DrawMenu()
            {
                float y = 270;
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    bool sel = i == selected;
                    if (sel)
                    {
                        Vector2 size = font.MeasureString(MenuItems[i]) * 1.5f;
                        sb.Draw(pixel, new Rectangle((int)(640 - size.X / 2 - 20), (int)y - 6, (int)size.X + 40, (int)size.Y + 8), new Color(170, 170, 255) * 0.85f);
                    }
                    DrawCentered(font, MenuItems[i], y, sel ? new Color(200, 0, 0) : Color.White, 1.5f, true);
                    y += 60;
                }
                DrawCentered(font, "Online play: one PC hosts, the others join with the host's IP address.", 560, Color.LightGray, 1f, false);
                DrawCentered(font, "[Enter]/(A) Select     [Esc]/(B) Quit", 640, Color.LightGray, 1f, false);
            }

            void DrawLobby()
            {
                NetSession s = Session;
                if (s == null)
                    return;
                DrawPanel(new Rectangle(140, 150, 1000, 470));
                float x = 180, y = 175;
                if (s.IsHost)
                {
                    DrawText(font, "HOSTING ONLINE GAME", new Vector2(x, y), new Color(255, 220, 120), 1.3f);
                    y += 45;
                    string ips = addresses.Count > 0 ? string.Join("   ", addresses) : "(no network found)";
                    DrawText(font, $"Other players join with:  {ips}   port {s.Port}", new Vector2(x, y), Color.White, 1f);
                    y += 28;
                    DrawText(font, $"Over the internet: forward TCP port {s.Port} to this PC and give them your public IP.", new Vector2(x, y), Color.LightGray, 1f);
                    y += 40;
                }
                else
                {
                    DrawText(font, "JOIN ONLINE GAME", new Vector2(x, y), new Color(255, 220, 120), 1.3f);
                    y += 45;
                    DrawText(font, s.Status, new Vector2(x, y), Color.White, 1f);
                    y += 68;
                }

                DrawText(font, "PLAYERS", new Vector2(x, y), new Color(255, 220, 120), 1f);
                y += 30;
                foreach (NetPeer p in s.Peers.Where(p => p.Connected))
                {
                    string slots = p.Slots.Count == 0 ? "no free controller slot" : "controller " + string.Join(", ", p.Slots.Select(n => (n + 1).ToString()));
                    string ping = p.Id == 0 ? "host" : p.PingMs >= 0 ? $"{p.PingMs} ms" : "...";
                    string you = p.Id == s.LocalPeerId ? "  (you)" : string.Empty;
                    DrawText(font, $"{p.Name}{you}", new Vector2(x + 20, y), Color.White, 1f);
                    DrawText(font, slots, new Vector2(x + 420, y), Color.LightGray, 1f);
                    DrawText(font, ping, new Vector2(x + 760, y), Color.LightGray, 1f);
                    y += 28;
                }

                float bottom = 560;
                int delayMs = (int)Math.Round(s.InputDelay * 1000.0 / 60.0);
                if (s.IsHost)
                {
                    DrawText(font, $"Input delay: {s.InputDelay} frames ({delayMs} ms)   [Left/Right] change - raise it if the game stutters", new Vector2(x, bottom - 40), Color.LightGray, 1f);
                    string start = s.CanStart ? "[Enter] Start game" : "Waiting for at least one other player...";
                    DrawText(font, start + "     [Esc] Cancel", new Vector2(x, bottom), s.CanStart ? Color.White : Color.LightGray, 1f);
                }
                else
                {
                    if (s.LocalPeerId >= 0)
                        DrawText(font, $"Input delay: {s.InputDelay} frames ({delayMs} ms)", new Vector2(x, bottom - 40), Color.LightGray, 1f);
                    DrawText(font, "[Esc] Leave", new Vector2(x, bottom), Color.White, 1f);
                }
            }

            void DrawPanel(Rectangle r)
            {
                sb.Draw(pixel, r, new Color(20, 24, 32) * 0.92f);
                Color c = new Color(200, 170, 90);
                sb.Draw(pixel, new Rectangle(r.X, r.Y, r.Width, 2), c);
                sb.Draw(pixel, new Rectangle(r.X, r.Bottom - 2, r.Width, 2), c);
                sb.Draw(pixel, new Rectangle(r.X, r.Y, 2, r.Height), c);
                sb.Draw(pixel, new Rectangle(r.Right - 2, r.Y, 2, r.Height), c);
            }

            void DrawCentered(SpriteFont f, string text, float y, Color color, float scale, bool shadow)
            {
                text = Sanitize(f, text);
                Vector2 size = f.MeasureString(text) * scale;
                var pos = new Vector2((float)Math.Round(640 - size.X / 2), y);
                if (shadow)
                    sb.DrawString(f, text, pos + new Vector2(2, 2), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                sb.DrawString(f, text, pos, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            void DrawWrapped(string text, Vector2 pos, float width, Color color)
            {
                string line = string.Empty;
                foreach (string word in text.Split(' '))
                {
                    string candidate = line.Length == 0 ? word : line + " " + word;
                    if (line.Length > 0 && font.MeasureString(Sanitize(font, candidate)).X > width)
                    {
                        DrawText(font, line, pos, color, 1f);
                        pos.Y += font.LineSpacing + 4;
                        line = word;
                    }
                    else
                    {
                        line = candidate;
                    }
                }
                DrawText(font, line, pos, color, 1f);
            }

            void DrawText(SpriteFont f, string text, Vector2 pos, Color color, float scale)
            {
                sb.DrawString(f, Sanitize(f, text ?? string.Empty), pos, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        private static string Sanitize(SpriteFont f, string s)
        {
            var chars = s.Select(c => f.Characters.Contains(c) ? c : (f.DefaultCharacter ?? (f.Characters.Contains('?') ? '?' : ' '))).ToArray();
            return new string(chars);
        }
    }
}
