using System.Collections.Generic;
using Adrift.Content.Common.UI;
using FontExtension;
using GameJam.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GameJam
{
    public partial class GameManager
    {
        private void LoadGameContent(LockingContentManager content)
        {
            /// IMPORTANT: **ALWAYS** use `content` instead of `Content` in
            /// this method.

            content.Load<Texture2D>("texture_particle_velocity");

            content.Load<Texture2D>("texture_background_stars_0");
            content.Load<Texture2D>("texture_background_stars_1");
            content.Load<Texture2D>("texture_background_stars_2");
            content.Load<Texture2D>("texture_background_stars_3");
            content.Load<Texture2D>("texture_background_stars_4");
            content.Load<Texture2D>("texture_background_stars_5");
            content.Load<Texture2D>("texture_background_stars_6");
            content.Load<Texture2D>("texture_background_stars_7");
            content.Load<Texture2D>("texture_background_stars_8");
            content.Load<Texture2D>("texture_background_stars_9");
            content.Load<Texture2D>("texture_background_stars_10");
            content.Load<Texture2D>("texture_background_stars_11");
            content.Load<Texture2D>("texture_background_stars_12");
            content.Load<Texture2D>("texture_background_stars_13");

            content.Load<Texture2D>("texture_background_parallax_test");

            content.Load<Texture2D>("texture_ui_button_released");
            content.Load<Texture2D>("texture_ui_button_hover");
            content.Load<Texture2D>("texture_ui_button_pressed");
            content.Load<Texture2D>("texture_ui_lobby_panel_background");
            content.Load<Texture2D>("texture_ui_dropdown_background");

            content.Load<Texture2D>("texture_input_keyboard_key_0");
            content.Load<Texture2D>("texture_input_keyboard_key_1");
            content.Load<Texture2D>("texture_input_keyboard_key_2");
            content.Load<Texture2D>("texture_input_keyboard_key_3");
            content.Load<Texture2D>("texture_input_keyboard_key_4");
            content.Load<Texture2D>("texture_input_keyboard_key_5");
            content.Load<Texture2D>("texture_input_keyboard_key_6");
            content.Load<Texture2D>("texture_input_keyboard_key_7");
            content.Load<Texture2D>("texture_input_keyboard_key_8");
            content.Load<Texture2D>("texture_input_keyboard_key_9");

            content.Load<Texture2D>("texture_input_keyboard_key_a");
            content.Load<Texture2D>("texture_input_keyboard_key_b");
            content.Load<Texture2D>("texture_input_keyboard_key_c");
            content.Load<Texture2D>("texture_input_keyboard_key_d");
            content.Load<Texture2D>("texture_input_keyboard_key_e");
            content.Load<Texture2D>("texture_input_keyboard_key_f");
            content.Load<Texture2D>("texture_input_keyboard_key_g");
            content.Load<Texture2D>("texture_input_keyboard_key_h");
            content.Load<Texture2D>("texture_input_keyboard_key_i");
            content.Load<Texture2D>("texture_input_keyboard_key_j");
            content.Load<Texture2D>("texture_input_keyboard_key_k");
            content.Load<Texture2D>("texture_input_keyboard_key_l");
            content.Load<Texture2D>("texture_input_keyboard_key_m");
            content.Load<Texture2D>("texture_input_keyboard_key_n");
            content.Load<Texture2D>("texture_input_keyboard_key_o");
            content.Load<Texture2D>("texture_input_keyboard_key_p");
            content.Load<Texture2D>("texture_input_keyboard_key_q");
            content.Load<Texture2D>("texture_input_keyboard_key_r");
            content.Load<Texture2D>("texture_input_keyboard_key_s");
            content.Load<Texture2D>("texture_input_keyboard_key_t");
            content.Load<Texture2D>("texture_input_keyboard_key_u");
            content.Load<Texture2D>("texture_input_keyboard_key_v");
            content.Load<Texture2D>("texture_input_keyboard_key_w");
            content.Load<Texture2D>("texture_input_keyboard_key_x");
            content.Load<Texture2D>("texture_input_keyboard_key_y");
            content.Load<Texture2D>("texture_input_keyboard_key_z");

            content.Load<Texture2D>("texture_input_keyboard_key_up");
            content.Load<Texture2D>("texture_input_keyboard_key_down");
            content.Load<Texture2D>("texture_input_keyboard_key_left");
            content.Load<Texture2D>("texture_input_keyboard_key_right");

            content.Load<Texture2D>("texture_input_keyboard_key_bracket_left");
            content.Load<Texture2D>("texture_input_keyboard_key_bracket_right");
            content.Load<Texture2D>("texture_input_keyboard_key_semicolon");
            content.Load<Texture2D>("texture_input_keyboard_key_quote");
            content.Load<Texture2D>("texture_input_keyboard_key_angle_left");
            content.Load<Texture2D>("texture_input_keyboard_key_angle_right");
            content.Load<Texture2D>("texture_input_keyboard_key_question");
            content.Load<Texture2D>("texture_input_keyboard_key_slash");
            content.Load<Texture2D>("texture_input_keyboard_key_minus");
            content.Load<Texture2D>("texture_input_keyboard_key_plus");
            content.Load<Texture2D>("texture_input_keyboard_key_space");

            content.Load<Texture2D>("texture_input_keyboard_key_blank");

            content.Load<Texture2D>("texture_input_gamepad_a");
            content.Load<Texture2D>("texture_input_gamepad_b");
            content.Load<Texture2D>("texture_input_gamepad_x");
            content.Load<Texture2D>("texture_input_gamepad_y");

            content.Load<Texture2D>("texture_input_gamepad_dpad_up");
            content.Load<Texture2D>("texture_input_gamepad_dpad_down");
            content.Load<Texture2D>("texture_input_gamepad_dpad_left");
            content.Load<Texture2D>("texture_input_gamepad_dpad_right");

            content.Load<Texture2D>("texture_input_gamepad_bumper_left");
            content.Load<Texture2D>("texture_input_gamepad_bumper_right");
            content.Load<Texture2D>("texture_input_gamepad_trigger_left");
            content.Load<Texture2D>("texture_input_gamepad_trigger_right");

            content.Load<Texture2D>("texture_input_gamepad_stick_left");
            content.Load<Texture2D>("texture_input_gamepad_stick_right");

            content.Load<SoundEffect>("sound_explosion");
            content.Load<SoundEffect>("sound_projectile_fired");
            content.Load<SoundEffect>("sound_projectile_bounce");

            content.Load<BitmapFont>("font_game_over");
            content.Load<Texture2D>("font_texture_game_over");
            content.Load<BitmapFont>("font_hyperspace");
            content.Load<Texture2D>("font_texture_hyperspace");
            content.Load<BitmapFont>("font_title");
            content.Load<Texture2D>("font_texture_title");

            content.Load<Effect>("effect_blur");
            content.Load<Effect>("effect_fxaa");

            content.Load<Effect>("effect_field_font");
            content.Load<FieldFont>("font_msdf_hyperspace");

            content.Load<List<WidgetPrototype>>("ui_main_menu");
            content.Load<List<WidgetPrototype>>("ui_options_menu");
            content.Load<List<WidgetPrototype>>("ui_pause_menu");
            content.Load<List<WidgetPrototype>>("ui_adrift_game_ui");

            content.Load<List<WidgetPrototype>>("ui_lobby_menu");
            content.Load<List<WidgetPrototype>>("ui_lobby_menu_join_instructions");

            content.Load<List<WidgetPrototype>>("ui_game_over");

            content.Load<List<WidgetPrototype>>("ui_test");
            content.Load<List<WidgetPrototype>>("ui_test2");
        }
    }
}
