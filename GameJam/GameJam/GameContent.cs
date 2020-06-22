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
            
            content.Load<TextureAtlas>("complete_texture_atlas");
            content.Load<Texture2D>("complete_texture_atlas_texture");

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

            content.Load<Texture2D>("texture_gravity_hole_circle");

            content.Load<SoundEffect>("sound_explosion");
            content.Load<SoundEffect>("sound_projectile_fired");
            content.Load<SoundEffect>("sound_projectile_bounce");

            content.Load<Effect>("effect_blur");
            content.Load<Effect>("effect_fxaa");
            content.Load<Effect>("effect_smaa");
            content.Load<Effect>("effect_negative");

            content.Load<Effect>("effect_field_font");
            content.Load<FieldFont>("font_msdf_hyperspace");

            content.Load<Effect>("effect_gpu_particle_velocity");

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
