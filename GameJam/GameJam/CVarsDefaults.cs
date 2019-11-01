using Microsoft.Xna.Framework;

namespace GameJam
{
    static partial class CVars
    {
        private static void CreateDefaultCVars()
        {
            /** GENERAL **/
            Create<float>("tick_frequency", 120, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("initial_window_width", 1280, CVarFlags.PRESERVE);
            Create<int>("initial_window_height", 720, CVarFlags.PRESERVE);

            // "Screen" is the internal units of the game.
            // Always use `screen_width` and `screen_height`.
            Create<float>("screen_width", 1280, CVarFlags.DEV_PRESERVE);
            Create<float>("screen_height", 720, CVarFlags.DEV_PRESERVE);

            /** GRAPHICS **/
            Create<bool>("graphics_fxaa", false, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<bool>("graphics_frame_smoothing", true, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<bool>("graphics_feathering", true, CVarFlags.PRESERVE);
            Create<float>("graphics_feathering_width", 0.2f, CVarFlags.PRESERVE);
            /**
             * If more than one are true; priority in order of highest
             * to lowest: fullscreen, borderless, windowed
             * **/
            Create<bool>("display_windowed", true, CVarFlags.PRESERVE);
            Create<bool>("display_borderless", false, CVarFlags.PRESERVE);
            Create<bool>("display_fullscreen", false, CVarFlags.PRESERVE);

            /** INPUT **/
            Create<float>("input_controller_deadzone", 0.1f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_thumbstick", 1, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_counter_clockwise", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_clockwise", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_pause", 16, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("input_shield_angular_speed", 5.026f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_primary_counter_clockwise", 65, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_keyboard_primary_clockwise", 68, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_secondary_counter_clockwise", 37, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_keyboard_secondary_clockwise", 39, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_pause", 27, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            /** GAMEPLAY **/
            Create<float>("player_shield_radius", 30.0f, CVarFlags.DEV_PRESERVE);
            Create<float>("player_shield_size", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("player_ship_size", 5, CVarFlags.DEV_PRESERVE);
            Create<int>("player_ship_max_health", 3, CVarFlags.DEV_PRESERVE);

            Create<bool>("god", false, CVarFlags.DEV_PRESERVE);

            Create<float>("kamikaze_enemy_speed", 100.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("kamikaze_enemy_rotational_speed", 1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("kamikaze_size", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("enemy_pushback_force", 120.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("shooting_enemy_size", 4, CVarFlags.DEV_PRESERVE);
            Create<float>("shooting_enemy_speed", 0, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("shooting_enemy_rotational_speed", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("shooting_enemy_projectile_speed", 120.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("shooting_enemy_projectile_bounces", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("shooting_enemy_projectile_ammo", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("gravity_enemy_size", 2, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_enemy_radius", 125.0f, CVarFlags.DEV_PRESERVE);
            Create<int>("gravity_hole_enemy_lifespan", 15, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_enemy_force", 170.0f, CVarFlags.DEV_PRESERVE);

            Create<float>("projectile_size", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_kamikaze_enemy_initial_period", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_kamikaze_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_kamikaze_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_shooting_enemy_initial_period", 10, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_shooting_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_shooting_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_gravity_enemy_initial_period", 15, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_gravity_enemy_period_multiplier", 0.999f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_gravity_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_laser_enemy_initial_period", 15, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_laser_enemy_period_multiplier", 0.9f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_laser_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("spawner_laser_enemy_max_entities", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_min_distance_away_from_player", 200.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("spawner_max_enemy_count", 50, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("laser_enemy_rotational_speed", 3f, CVarFlags.DEV_PRESERVE);
            Create<float>("laser_enemy_size", 4f, CVarFlags.DEV_PRESERVE);
            Create<float>("laser_enemy_warm_up_anim_duration", 0.1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_warm_up_duration", 2, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_warm_up_thickness", 1.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_fire_duration", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_fire_frequency", 80, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_fire_thickness", 5, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_fire_thickness_variability", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_fire_initial_thickness_decay_factor", 25, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_fire_closing_envelope_decay_factor", 20, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_spawn_wait_period", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("laser_enemy_successive_wait_period", 7, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            /** MENU **/
            Create<float>("entity_background_spawner_min", 1f, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_spawner_max", 2, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_spawner_x", CVars.Get<float>("screen_width") / 2 * 1.05f, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_spawner_y_min", CVars.Get<float>("screen_height") * -0.49f, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_spawner_y_max", CVars.Get<float>("screen_height") * 0.49f, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_spawner_destruction_x", -CVars.Get<float>("entity_background_spawner_x"), CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_entity_speed_min", 50, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_entity_speed_max", 175, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_entity_angular_speed_min", -MathHelper.PiOver2, CVarFlags.DEV_PRESERVE);
            Create<float>("entity_background_entity_angular_speed_max", MathHelper.PiOver2, CVarFlags.DEV_PRESERVE);

            /** COLORS **/
            Create<Color>("color_player_ship", Color.White, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_player_shield", Color.SpringGreen, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_kamikaze_enemy", Color.Violet, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_shooting_enemy", Color.Cyan, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_laser_enemy", Color.Gold, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_gravity_hold_enemy", Color.Purple, CVarFlags.DEV_PRESERVE);

            Create<Color>("color_projectile", Color.Red, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_laser_beam", Color.Red, CVarFlags.DEV_PRESERVE);

            Create<Color>("color_playfield", Color.Green, CVarFlags.DEV_PRESERVE);

            /** DEBUG **/
            Create<bool>("debug_show_cvar_viewer", false, CVarFlags.LIVE_RELOAD);
            Create<bool>("debug_show_playback_controls", false, CVarFlags.LIVE_RELOAD);
            Create<bool>("debug_pause_game_updates", false, CVarFlags.LIVE_RELOAD);
            Create<float>("debug_update_time_scale", 1.0f, CVarFlags.LIVE_RELOAD);
            Create<float>("debug_game_step_period", 1 / 60.0f, CVarFlags.LIVE_RELOAD);
            Create<bool>("debug_show_console", false, CVarFlags.LIVE_RELOAD);
            Create<int>("debug_max_console_entries", 1000, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<string>("debug_console_filter", @"Mouse[A-z]+|Keyboard[A-z]+|Component[A-z]+|Entity[A-z]+|CollisionEnd[A-z]+", CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<bool>("debug_show_statistics", false, CVarFlags.LIVE_RELOAD);
            Create<int>("debug_statistics_average_between_ticks_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("debug_statistics_average_between_frames_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("debug_statistics_average_update_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("debug_statistics_average_draw_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            /** PARTICLES **/
            Create<int>("particle_explosion_count", 150, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_strength", 750, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_decay_multiplier", 0.96f, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_variety_min", 0.1f, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_variety_max", 1f, CVarFlags.DEV_PRESERVE);

            /** RESOURCES **/
            Create<string>("texture_player_ship", "textures/PlayerShip", CVarFlags.PRESERVE);
            Create<string>("texture_player_shield", "textures/shield", CVarFlags.PRESERVE);
            Create<string>("texture_explosion", "textures/Explosion", CVarFlags.PRESERVE);
            Create<string>("texture_kamikaze", "textures/Kamikaze", CVarFlags.PRESERVE);
            Create<string>("texture_shooter_enemy", "textures/ShootingEnemy", CVarFlags.PRESERVE);
            Create<string>("texture_enemy_bullet", "textures/EnemyBullet", CVarFlags.PRESERVE);

            Create<string>("texture_title_with_instructions", "textures/TitleInstructions", CVarFlags.PRESERVE);
            Create<string>("texture_title_without_instructions", "textures/TitleNoInstructions", CVarFlags.PRESERVE);

            Create<string>("texture_particle_velocity", "textures/particles/VelocityParticle", CVarFlags.PRESERVE);

            Create<string>("texture_background_stars_0", "textures/Stars/000", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_1", "textures/Stars/001", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_2", "textures/Stars/002", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_3", "textures/Stars/003", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_4", "textures/Stars/004", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_5", "textures/Stars/005", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_6", "textures/Stars/006", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_7", "textures/Stars/007", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_8", "textures/Stars/008", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_9", "textures/Stars/009", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_10", "textures/Stars/010", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_11", "textures/Stars/011", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_12", "textures/Stars/012", CVarFlags.PRESERVE);
            Create<string>("texture_background_stars_13", "textures/Stars/013", CVarFlags.PRESERVE);

            Create<string>("texture_background_parallax_test", "textures/ParalaxTestBackground", CVarFlags.PRESERVE);

            Create<string>("texture_ui_button_released", "textures/ui/button_up_background", CVarFlags.PRESERVE);
            Create<string>("texture_ui_button_hover", "textures/ui/button_over_background", CVarFlags.PRESERVE);
            Create<string>("texture_ui_button_pressed", "textures/ui/button_down_background", CVarFlags.PRESERVE);
            Create<string>("texture_ui_lobby_panel_background", "textures/ui/LobbyPanelBackground", CVarFlags.PRESERVE);

            Create<string>("texture_input_keyboard_a_key", "textures/input/Keyboard_Black_A", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_d_key", "textures/input/Keyboard_Black_D", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_left_arrow_key", "textures/input/Keyboard_Black_Arrow_Left", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_right_arrow_key", "textures/input/Keyboard_Black_Arrow_Right", CVarFlags.PRESERVE);

            Create<string>("texture_input_controller_a_button", "textures/input/XBoxOne_A", CVarFlags.PRESERVE);
            Create<string>("texture_input_controller_b_button", "textures/input/XBoxOne_B", CVarFlags.PRESERVE);
            Create<string>("texture_input_controller_left_bumper", "textures/input/XBoxOne_LB", CVarFlags.PRESERVE);
            Create<string>("texture_input_controller_right_bumper", "textures/input/XBoxOne_RB", CVarFlags.PRESERVE);

            Create<string>("sound_explosion", "sounds/Explosion_Sound", CVarFlags.PRESERVE);
            Create<string>("sound_projectile_fired", "sounds/Laser_Shot", CVarFlags.PRESERVE);
            Create<string>("sound_projectile_bounce", "sounds/Proj_Bounce", CVarFlags.PRESERVE);

            Create<string>("font_game_over", "fonts/Intro", CVarFlags.PRESERVE);
            Create<string>("font_hyperspace", "fonts/Main", CVarFlags.PRESERVE);

            Create<string>("effect_blur", "effects/Blur", CVarFlags.PRESERVE);
            Create<string>("effect_fxaa", "effects/FXAA", CVarFlags.PRESERVE);

            Create<string>("ui_pause_menu", "ui/PauseMenu", CVarFlags.PRESERVE);
        }
    }
}
