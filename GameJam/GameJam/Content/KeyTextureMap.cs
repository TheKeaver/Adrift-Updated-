using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Content
{
    public class KeyTextureMap
    {
        private Dictionary<Keys, string> _dictionary = new Dictionary<Keys, string>();

        public KeyTextureMap()
        {
            BuildMap();
        }

        private void BuildMap()
        {
            _dictionary.Add(Keys.D0, "texture_input_keyboard_key_0");
            _dictionary.Add(Keys.D1, "texture_input_keyboard_key_1");
            _dictionary.Add(Keys.D2, "texture_input_keyboard_key_2");
            _dictionary.Add(Keys.D3, "texture_input_keyboard_key_3");
            _dictionary.Add(Keys.D4, "texture_input_keyboard_key_4");
            _dictionary.Add(Keys.D5, "texture_input_keyboard_key_5");
            _dictionary.Add(Keys.D6, "texture_input_keyboard_key_6");
            _dictionary.Add(Keys.D7, "texture_input_keyboard_key_7");
            _dictionary.Add(Keys.D8, "texture_input_keyboard_key_8");
            _dictionary.Add(Keys.D9, "texture_input_keyboard_key_9");

            _dictionary.Add(Keys.A, "texture_input_keyboard_key_a");
            _dictionary.Add(Keys.B, "texture_input_keyboard_key_b");
            _dictionary.Add(Keys.C, "texture_input_keyboard_key_c");
            _dictionary.Add(Keys.D, "texture_input_keyboard_key_d");
            _dictionary.Add(Keys.E, "texture_input_keyboard_key_e");
            _dictionary.Add(Keys.F, "texture_input_keyboard_key_f");
            _dictionary.Add(Keys.G, "texture_input_keyboard_key_g");
            _dictionary.Add(Keys.H, "texture_input_keyboard_key_h");
            _dictionary.Add(Keys.I, "texture_input_keyboard_key_i");
            _dictionary.Add(Keys.J, "texture_input_keyboard_key_j");
            _dictionary.Add(Keys.K, "texture_input_keyboard_key_k");
            _dictionary.Add(Keys.L, "texture_input_keyboard_key_l");
            _dictionary.Add(Keys.M, "texture_input_keyboard_key_m");
            _dictionary.Add(Keys.N, "texture_input_keyboard_key_n");
            _dictionary.Add(Keys.O, "texture_input_keyboard_key_o");
            _dictionary.Add(Keys.P, "texture_input_keyboard_key_p");
            _dictionary.Add(Keys.Q, "texture_input_keyboard_key_q");
            _dictionary.Add(Keys.R, "texture_input_keyboard_key_r");
            _dictionary.Add(Keys.S, "texture_input_keyboard_key_s");
            _dictionary.Add(Keys.T, "texture_input_keyboard_key_t");
            _dictionary.Add(Keys.U, "texture_input_keyboard_key_u");
            _dictionary.Add(Keys.V, "texture_input_keyboard_key_v");
            _dictionary.Add(Keys.W, "texture_input_keyboard_key_w");
            _dictionary.Add(Keys.X, "texture_input_keyboard_key_x");
            _dictionary.Add(Keys.Y, "texture_input_keyboard_key_y");
            _dictionary.Add(Keys.Z, "texture_input_keyboard_key_z");

            _dictionary.Add(Keys.Up, "texture_input_keyboard_key_up");
            _dictionary.Add(Keys.Down, "texture_input_keyboard_key_down");
            _dictionary.Add(Keys.Left, "texture_input_keyboard_key_left");
            _dictionary.Add(Keys.Right, "texture_input_keyboard_key_right");

            _dictionary.Add(Keys.OemOpenBrackets, "texture_input_keyboard_key_bracket_left");
            _dictionary.Add(Keys.OemCloseBrackets, "texture_input_keyboard_key_bracket_right");
            _dictionary.Add(Keys.OemSemicolon, "texture_input_keyboard_key_semicolon");
            _dictionary.Add(Keys.OemQuotes, "texture_input_keyboard_key_quote");
            _dictionary.Add(Keys.OemComma, "texture_input_keyboard_key_mark_left");
            _dictionary.Add(Keys.OemPeriod, "texture_input_keyboard_key_mark_right");
            _dictionary.Add(Keys.OemQuestion, "texture_input_keyboard_key_question");
            _dictionary.Add(Keys.OemBackslash, "texture_input_keyboard_key_slash");
            _dictionary.Add(Keys.OemMinus, "texture_input_keyboard_key_minus");
            _dictionary.Add(Keys.OemPlus, "texture_input_keyboard_key_plus");
            _dictionary.Add(Keys.Space, "texture_input_keyboard_key_space");
        }

        public void CacheAll(ContentManager content)
        {
            foreach(Keys key in _dictionary.Keys)
            {
                content.Load<TextureAtlas>("complete_texture_atlas").GetRegion(_dictionary[key]);
            }
        }

        public string At(Keys key)
        {
            return _dictionary[key];
        }

        public string this[Keys key]
        {
            get
            {
                return _dictionary[key];
            }
        }
    }
}
