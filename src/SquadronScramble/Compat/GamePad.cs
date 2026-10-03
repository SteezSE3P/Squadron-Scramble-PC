// PC input layer. The game only reads Xbox controllers; this class shadows
// Microsoft.Xna.Framework.Input.GamePad for the game code (same namespace wins over
// using-directives) and adds the keyboard as an extra "virtual controller".
//
// The keyboard takes the first controller slot (One..Four) that has no physical
// gamepad plugged in. Like a real pad it can be shared by two pilots:
//
//   Left pilot  (left half of the pad):  W A S D move/turn, Left Shift = fire (LT), Q = LB
//   Right pilot (right half of the pad): Arrow keys move/turn, Right Ctrl = fire (RT), Right Shift = RB
//   Buttons: Space = A, Escape = B, E = X, Tab = Y, Enter = Start, Backspace = Back
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaGamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace SquadronScramble
{
    public static class GamePad
    {
        /// <summary>Set to false to stop the keyboard acting as a controller.</summary>
        public static bool KeyboardEnabled = true;

        public static PlayerIndex? KeyboardPlayerIndex
        {
            get
            {
                if (!KeyboardEnabled)
                    return null;
                for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
                {
                    if (!XnaGamePad.GetState(i).IsConnected)
                        return i;
                }
                return null;
            }
        }

        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            return Filter(playerIndex, XnaGamePad.GetState(playerIndex));
        }

        public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZone)
        {
            return Filter(playerIndex, XnaGamePad.GetState(playerIndex, deadZone));
        }

        private static GamePadState Filter(PlayerIndex playerIndex, GamePadState pad)
        {
            if (!pad.IsConnected)
            {
                if (KeyboardPlayerIndex != playerIndex)
                    return pad;
                pad = GetKeyboardState();
            }
            // Don't let the press that closed a text box / message box leak into the game.
            if (Microsoft.Xna.Framework.GamerServices.Guide.IsInputBlocked)
                return Neutral;
            return pad;
        }

        private static readonly GamePadState Neutral = new GamePadState(
            new GamePadThumbSticks(Vector2.Zero, Vector2.Zero),
            new GamePadTriggers(0f, 0f),
            new GamePadButtons(0),
            new GamePadDPad(ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));

        public static GamePadCapabilities GetCapabilities(PlayerIndex playerIndex)
        {
            return XnaGamePad.GetCapabilities(playerIndex);
        }

        public static bool SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
        {
            return XnaGamePad.SetVibration(playerIndex, leftMotor, rightMotor);
        }

        private static GamePadState GetKeyboardState()
        {
            KeyboardState k = Keyboard.GetState();
            var left = new Vector2(Axis(k, Keys.A, Keys.D), Axis(k, Keys.S, Keys.W));
            var right = new Vector2(Axis(k, Keys.Left, Keys.Right), Axis(k, Keys.Down, Keys.Up));

            Buttons buttons = 0;
            if (k.IsKeyDown(Keys.Space)) buttons |= Buttons.A;
            if (k.IsKeyDown(Keys.Escape)) buttons |= Buttons.B;
            if (k.IsKeyDown(Keys.E)) buttons |= Buttons.X;
            if (k.IsKeyDown(Keys.Tab)) buttons |= Buttons.Y;
            if (k.IsKeyDown(Keys.Enter) && !k.IsKeyDown(Keys.LeftAlt) && !k.IsKeyDown(Keys.RightAlt)) buttons |= Buttons.Start;
            if (k.IsKeyDown(Keys.Back)) buttons |= Buttons.Back;
            if (k.IsKeyDown(Keys.Q)) buttons |= Buttons.LeftShoulder;
            if (k.IsKeyDown(Keys.RightShift)) buttons |= Buttons.RightShoulder;

            float leftTrigger = k.IsKeyDown(Keys.LeftShift) ? 1f : 0f;
            float rightTrigger = k.IsKeyDown(Keys.RightControl) ? 1f : 0f;

            return new GamePadState(
                new GamePadThumbSticks(left, right),
                new GamePadTriggers(leftTrigger, rightTrigger),
                new GamePadButtons(buttons),
                new GamePadDPad(ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));
        }

        private static float Axis(KeyboardState k, Keys negative, Keys positive)
        {
            float v = 0f;
            if (k.IsKeyDown(negative)) v -= 1f;
            if (k.IsKeyDown(positive)) v += 1f;
            return v;
        }
    }
}
