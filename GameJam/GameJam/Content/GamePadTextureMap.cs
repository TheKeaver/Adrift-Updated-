using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Content
{
    public class GamePadTextureMap
    {
        private Dictionary<Buttons, string> _dictionary = new Dictionary<Buttons, string>();

        public GamePadTextureMap()
        {
            BuildMap();
        }

        private void BuildMap()
        {
            _dictionary.Add(Buttons.A, "texture_input_gamepad_a");
            _dictionary.Add(Buttons.B, "texture_input_gamepad_b");
            _dictionary.Add(Buttons.X, "texture_input_gamepad_x");
            _dictionary.Add(Buttons.Y, "texture_input_gamepad_y");

            _dictionary.Add(Buttons.DPadUp, "texture_input_gamepad_dpad_up");
            _dictionary.Add(Buttons.DPadDown, "texture_input_gamepad_dpad_down");
            _dictionary.Add(Buttons.DPadLeft, "texture_input_gamepad_dpad_left");
            _dictionary.Add(Buttons.DPadRight, "texture_input_gamepad_dpad_right");

            _dictionary.Add(Buttons.LeftShoulder, "texture_input_gamepad_bumper_left");
            _dictionary.Add(Buttons.RightShoulder, "texture_input_gamepad_bumper_right");
            _dictionary.Add(Buttons.LeftTrigger, "texture_input_gamepad_trigger_left");
            _dictionary.Add(Buttons.RightTrigger, "texture_input_gamepad_trigger_right");

            _dictionary.Add(Buttons.LeftThumbstickUp, "texture_input_gamepad_stick_left");
            _dictionary.Add(Buttons.LeftThumbstickDown, "texture_input_gamepad_stick_left");
            _dictionary.Add(Buttons.LeftThumbstickLeft, "texture_input_gamepad_stick_left");
            _dictionary.Add(Buttons.LeftThumbstickRight, "texture_input_gamepad_stick_left");
            _dictionary.Add(Buttons.LeftStick, "texture_input_gamepad_stick_left");
            _dictionary.Add(Buttons.RightThumbstickUp, "texture_input_gamepad_stick_right");
            _dictionary.Add(Buttons.RightThumbstickDown, "texture_input_gamepad_stick_right");
            _dictionary.Add(Buttons.RightThumbstickLeft, "texture_input_gamepad_stick_right");
            _dictionary.Add(Buttons.RightThumbstickRight, "texture_input_gamepad_stick_right");
            _dictionary.Add(Buttons.RightStick, "texture_input_gamepad_stick_right");
        }

        public void CacheAll(ContentManager content)
        {
            foreach (Buttons button in _dictionary.Keys)
            {
                content.Load<Texture2D>(_dictionary[button]);
            }
        }

        public string At(Buttons button)
        {
            return _dictionary[button];
        }

        public string this[Buttons button]
        {
            get
            {
                return _dictionary[button];
            }
        }
    }
}
