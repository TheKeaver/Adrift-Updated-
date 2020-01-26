using System.Collections.Generic;
using Adrift.Content.Common.UI;
using FontExtension;
using GameJam.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace GameJam
{
    public partial class GameManager
    {
        private void LoadGameContent(LockingContentManager content)
        {
            /// IMPORTANT: **ALWAYS** use `content` instead of `Content` in
            /// this method.

            //content.Load<Texture2D>("texture_particle_velocity");

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

            content.Load<TextureAtlas>("complete_texture_atlas");
            content.Load<Texture2D>("complete_texture_atlas_texture");

            /** BEGIN CONTENT TO BE PUT INTO TEXTURE ATLAS **/
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
            /** END CONTENT TO BE PUT INTO TEXTURE ATLAS **/

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
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_32");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_33");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_34");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_35");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_36");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_37");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_38");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_39");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_40");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_41");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_42");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_43");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_44");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_45");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_46");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_47");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_48");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_49");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_50");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_51");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_52");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_53");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_54");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_55");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_56");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_57");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_58");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_59");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_60");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_61");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_62");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_63");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_64");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_65");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_66");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_67");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_68");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_69");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_70");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_71");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_72");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_73");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_74");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_75");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_76");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_77");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_78");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_79");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_80");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_81");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_82");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_83");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_84");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_85");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_86");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_87");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_88");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_89");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_90");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_91");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_92");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_93");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_94");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_95");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_96");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_97");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_98");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_99");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_100");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_101");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_102");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_103");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_104");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_105");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_106");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_107");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_108");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_109");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_110");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_111");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_112");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_113");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_114");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_115");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_116");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_117");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_118");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_119");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_120");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_121");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_122");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_123");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_124");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_125");
            content.Load<Texture2D>("font_msdf_hyperspace_texture_character_126");

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
