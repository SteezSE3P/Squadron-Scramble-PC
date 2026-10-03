// PC replacement for the Xbox 360-only Microsoft.Xna.Framework.GamerServices API.
// Only the parts Squadron Scramble uses are provided: trial mode (always the full game),
// the on-screen keyboard (used to rename pilots) and message boxes. The Xbox Guide
// popups are emulated with a simple overlay drawn on top of the game.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using SquadronScramble.Net;
using XnaGamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace Microsoft.Xna.Framework.GamerServices
{
    public readonly struct GuideRequestInfo
    {
        public GuideRequestInfo(bool active, PlayerIndex player)
        {
            Active = active;
            Player = player;
        }

        public bool Active { get; }
        public PlayerIndex Player { get; }
    }

    public enum MessageBoxIcon
    {
        None,
        Error,
        Warning,
        Alert
    }

    public static class Guide
    {
        internal static GuideRequest Active;

        internal static bool BlockUntilReleased;

        /// <summary>Content asset used to draw the overlay text.</summary>
        public static string FontAssetName = "mySpriteFont1";

        /// <summary>
        /// True while the overlay is open, and after it closes until the keys/buttons used to
        /// close it are released (on Xbox the Guide swallowed that press; the game must not see it).
        /// </summary>
        public static bool IsInputBlocked
        {
            get
            {
                if (Active != null)
                    return true;
                if (!BlockUntilReleased)
                    return false;
                if (AnyInputHeld())
                    return true;
                BlockUntilReleased = false;
                return false;
            }
        }

        private static bool AnyInputHeld()
        {
            if (Keyboard.GetState().GetPressedKeyCount() > 0)
                return true;
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                GamePadState pad = GamePad.GetState(i);
                if (pad.IsConnected && (pad.IsButtonDown(Buttons.A) || pad.IsButtonDown(Buttons.B) || pad.IsButtonDown(Buttons.Start) || pad.IsButtonDown(Buttons.Back)))
                    return true;
            }
            return false;
        }

        public static bool SimulateTrialMode { get; set; }

        /// <summary>The PC port is always the full game.</summary>
        public static bool IsTrialMode => false;

        public static bool IsVisible => Active != null;

        public static GuideRequestInfo ActiveRequestPlayer => new GuideRequestInfo(Active != null, Active?.Player ?? PlayerIndex.One);

        /// <summary>
        /// Online play: the popup's result, delivered to every PC on the same frame. Only the PC
        /// that owns the controller interacts with the popup; it submits the result to the session.
        /// </summary>
        public static void CompleteFromNetwork(object result)
        {
            Active?.Finish(result);
        }

        internal static NetSession Net => NetSession.Current != null && NetSession.Current.InGame ? NetSession.Current : null;

        public static IAsyncResult BeginShowKeyboardInput(PlayerIndex player, string title, string description, string defaultText, AsyncCallback callback, object state)
        {
            return BeginShowKeyboardInput(player, title, description, defaultText, callback, state, false);
        }

        public static IAsyncResult BeginShowKeyboardInput(PlayerIndex player, string title, string description, string defaultText, AsyncCallback callback, object state, bool usePasswordMode)
        {
            if (Active != null)
                throw new InvalidOperationException("The Guide is already visible.");
            Active = new GuideRequest(callback, state)
            {
                Kind = GuideRequestKind.Keyboard,
                Player = player,
                Title = title ?? string.Empty,
                Description = description ?? string.Empty,
                Password = usePasswordMode
            };
            Active.Text.Append(defaultText ?? string.Empty);
            return Active.AsyncResult;
        }

        public static string EndShowKeyboardInput(IAsyncResult result)
        {
            return (string)((CompletedAsyncResult)result).Result;
        }

        public static IAsyncResult BeginShowMessageBox(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
        {
            return BeginShowMessageBox(PlayerIndex.One, title, text, buttons, focusButton, icon, callback, state);
        }

        public static IAsyncResult BeginShowMessageBox(PlayerIndex player, string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
        {
            if (Active != null)
                throw new InvalidOperationException("The Guide is already visible.");
            Active = new GuideRequest(callback, state)
            {
                Kind = GuideRequestKind.MessageBox,
                Player = player,
                Title = title ?? string.Empty,
                Description = text ?? string.Empty,
                Buttons = (buttons ?? Enumerable.Empty<string>()).ToArray(),
                Selected = focusButton
            };
            return Active.AsyncResult;
        }

        public static int? EndShowMessageBox(IAsyncResult result)
        {
            return (int?)((CompletedAsyncResult)result).Result;
        }
    }

    internal enum GuideRequestKind
    {
        Keyboard,
        MessageBox
    }

    internal sealed class GuideRequest
    {
        private readonly AsyncCallback callback;

        public GuideRequest(AsyncCallback callback, object state)
        {
            this.callback = callback;
            AsyncResult = new CompletedAsyncResult { AsyncState = state, IsCompleted = false };
        }

        public CompletedAsyncResult AsyncResult { get; }
        public GuideRequestKind Kind;
        public PlayerIndex Player;
        public string Title;
        public string Description;
        public bool Password;
        public readonly StringBuilder Text = new StringBuilder();
        public string[] Buttons = Array.Empty<string>();
        public int Selected;
        public bool Submitted; // online: result sent, waiting for it to come back

        public void Finish(object result)
        {
            Guide.Active = null;
            Guide.BlockUntilReleased = true;
            AsyncResult.Result = result;
            AsyncResult.SetCompleted();
            callback?.Invoke(AsyncResult);
        }
    }

    /// <summary>
    /// Hosts the emulated Guide overlay. Add it to Game.Components (the game already does this).
    /// </summary>
    public class GamerServicesComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D pixel;
        private KeyboardState prevKeys;
        private GamePadState prevPad;
        private bool waitForRelease = true;
        private GuideRequest lastRequest;

        public GamerServicesComponent(Game game) : base(game)
        {
            DrawOrder = int.MaxValue;
            UpdateOrder = int.MinValue;
            game.Window.TextInput += OnTextInput;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            try
            {
                font = Game.Content.Load<SpriteFont>(Guide.FontAssetName);
            }
            catch (Exception)
            {
                font = null;
            }
        }

        /// <summary>Whether this PC can interact with the popup, and which local pad drives it.</summary>
        private static bool IsOwner(GuideRequest req, out PlayerIndex localPad)
        {
            localPad = req.Player;
            NetSession net = Guide.Net;
            if (net == null)
                return true;
            PlayerIndex? local = net.LocalPadForSlot(req.Player);
            if (!local.HasValue || req.Submitted)
                return false;
            localPad = local.Value;
            return true;
        }

        private static void Complete(GuideRequest req, object result)
        {
            NetSession net = Guide.Net;
            if (net == null)
            {
                req.Finish(result);
                return;
            }
            req.Submitted = true;
            net.SubmitGuideResult(result);
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            GuideRequest req = Guide.Active;
            if (req == null || req.Kind != GuideRequestKind.Keyboard || waitForRelease || !IsOwner(req, out _))
                return;
            char c = e.Character;
            if (char.IsControl(c))
                return;
            if (font != null && !font.Characters.Contains(c))
                return;
            if (req.Text.Length < 32)
                req.Text.Append(c);
        }

        public override void Update(GameTime gameTime)
        {
            GuideRequest req = Guide.Active;
            KeyboardState keys = Keyboard.GetState();
            bool owner = false;
            PlayerIndex localPad = PlayerIndex.One;
            if (req != null)
                owner = IsOwner(req, out localPad);
            GamePadState pad = owner ? XnaGamePad.GetState(localPad) : default;

            if (req != lastRequest)
            {
                // Ignore whatever is held down when the overlay opens (the game opens it on a button press).
                waitForRelease = true;
                lastRequest = req;
            }

            if (req != null && owner)
            {
                if (waitForRelease)
                {
                    if (!AnyHeld(keys, pad))
                        waitForRelease = false;
                }
                else
                {
                    bool accept = Pressed(keys, Keys.Enter) || Pressed(pad, Buttons.A) || Pressed(pad, Buttons.Start);
                    bool cancel = Pressed(keys, Keys.Escape) || Pressed(pad, Buttons.B) || Pressed(pad, Buttons.Back);

                    if (req.Kind == GuideRequestKind.Keyboard)
                    {
                        if (Pressed(keys, Keys.Back) && req.Text.Length > 0)
                            req.Text.Length--;
                        if (accept)
                            Complete(req, req.Text.ToString());
                        else if (cancel)
                            Complete(req, null);
                    }
                    else
                    {
                        int count = Math.Max(1, req.Buttons.Length);
                        if (Pressed(keys, Keys.Left) || Pressed(keys, Keys.Up) || Pressed(pad, Buttons.DPadLeft) || Pressed(pad, Buttons.DPadUp))
                            req.Selected = (req.Selected + count - 1) % count;
                        if (Pressed(keys, Keys.Right) || Pressed(keys, Keys.Down) || Pressed(pad, Buttons.DPadRight) || Pressed(pad, Buttons.DPadDown))
                            req.Selected = (req.Selected + 1) % count;
                        if (accept)
                            Complete(req, req.Buttons.Length > 0 ? (object)req.Selected : null);
                        else if (cancel)
                            Complete(req, null);
                    }
                }
            }

            prevKeys = keys;
            prevPad = pad;
        }

        private static bool AnyHeld(KeyboardState keys, GamePadState pad)
        {
            return keys.GetPressedKeyCount() > 0
                || pad.IsButtonDown(Buttons.A) || pad.IsButtonDown(Buttons.B)
                || pad.IsButtonDown(Buttons.Start) || pad.IsButtonDown(Buttons.Back);
        }

        private bool Pressed(KeyboardState keys, Keys key) => keys.IsKeyDown(key) && !prevKeys.IsKeyDown(key);

        private bool Pressed(GamePadState pad, Buttons button) => pad.IsButtonDown(button) && !prevPad.IsButtonDown(button);

        public override void Draw(GameTime gameTime)
        {
            GuideRequest req = Guide.Active;
            if (req == null || font == null)
                return;

            Viewport vp = GraphicsDevice.Viewport;
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, new Rectangle(0, 0, vp.Width, vp.Height), Color.Black * 0.6f);

            int boxHeight = req.Kind == GuideRequestKind.Keyboard ? font.LineSpacing * 5 + 80 : font.LineSpacing * 6 + 80;
            var box = new Rectangle(vp.Width / 2 - 360, vp.Height / 2 - boxHeight / 2, 720, boxHeight);
            spriteBatch.Draw(pixel, box, new Color(20, 24, 32) * 0.95f);
            DrawFrame(box, new Color(200, 170, 90));

            float y = box.Y + 20;
            NetSession net = Guide.Net;
            if (net != null && (req.Submitted || !net.IsLocalSlot(req.Player)))
            {
                string who = req.Submitted ? "Sending..." : $"Waiting for {net.SlotOwnerName(req.Player)}...";
                DrawText(req.Title, new Vector2(box.X + 30, y), new Color(255, 220, 120), box.Width - 60);
                y += font.LineSpacing + 10;
                DrawText(who, new Vector2(box.X + 30, y), Color.White, box.Width - 60);
                spriteBatch.End();
                return;
            }
            DrawText(req.Title, new Vector2(box.X + 30, y), new Color(255, 220, 120), box.Width - 60);
            y += font.LineSpacing + 10;
            y = DrawWrapped(req.Description, new Vector2(box.X + 30, y), Color.White, box.Width - 60);
            y += 15;

            if (req.Kind == GuideRequestKind.Keyboard)
            {
                var field = new Rectangle(box.X + 30, (int)y, box.Width - 60, font.LineSpacing + 16);
                spriteBatch.Draw(pixel, field, Color.Black);
                DrawFrame(field, Color.Gray);
                string shown = req.Password ? new string('*', req.Text.Length) : req.Text.ToString();
                bool caret = (gameTime.TotalGameTime.TotalSeconds % 1.0) < 0.5;
                DrawText(shown + (caret ? "_" : " "), new Vector2(field.X + 10, field.Y + 8), Color.White, field.Width - 20);
                DrawText("[Enter] OK   [Esc] Cancel", new Vector2(box.X + 30, box.Bottom - font.LineSpacing - 15), Color.LightGray, box.Width - 60);
            }
            else
            {
                float x = box.X + 30;
                float by = box.Bottom - font.LineSpacing - 25;
                for (int i = 0; i < req.Buttons.Length; i++)
                {
                    string label = req.Buttons[i];
                    Vector2 size = font.MeasureString(label);
                    var r = new Rectangle((int)x - 10, (int)by - 6, (int)size.X + 20, (int)size.Y + 12);
                    spriteBatch.Draw(pixel, r, i == req.Selected ? new Color(200, 170, 90) : new Color(60, 60, 70));
                    DrawText(label, new Vector2(x, by), i == req.Selected ? Color.Black : Color.White, box.Width);
                    x += size.X + 50;
                }
            }
            spriteBatch.End();
        }

        private void DrawFrame(Rectangle r, Color c)
        {
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y, r.Width, 2), c);
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Bottom - 2, r.Width, 2), c);
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y, 2, r.Height), c);
            spriteBatch.Draw(pixel, new Rectangle(r.Right - 2, r.Y, 2, r.Height), c);
        }

        private string Sanitize(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                if (c == '\n' || font.Characters.Contains(c))
                    sb.Append(c);
                else if (font.DefaultCharacter.HasValue)
                    sb.Append(font.DefaultCharacter.Value);
                else if (font.Characters.Contains('?'))
                    sb.Append('?');
            }
            return sb.ToString();
        }

        private void DrawText(string text, Vector2 pos, Color color, float maxWidth)
        {
            text = Sanitize(text.Replace("\r", string.Empty));
            float scale = 1f;
            float w = font.MeasureString(text).X;
            if (w > maxWidth && w > 0)
                scale = maxWidth / w;
            spriteBatch.DrawString(font, text, pos, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private float DrawWrapped(string text, Vector2 pos, Color color, float maxWidth)
        {
            foreach (string paragraph in text.Replace("\r", string.Empty).Split('\n'))
            {
                string line = string.Empty;
                foreach (string word in paragraph.Split(' '))
                {
                    string candidate = line.Length == 0 ? word : line + " " + word;
                    if (line.Length > 0 && font.MeasureString(Sanitize(candidate)).X > maxWidth)
                    {
                        DrawText(line, pos, color, maxWidth);
                        pos.Y += font.LineSpacing;
                        line = word;
                    }
                    else
                    {
                        line = candidate;
                    }
                }
                DrawText(line, pos, color, maxWidth);
                pos.Y += font.LineSpacing;
            }
            return pos.Y;
        }
    }
}
