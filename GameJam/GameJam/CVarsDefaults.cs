using Microsoft.Xna.Framework;

namespace GameJam
{
    static partial class CVars
    {
        private static void CreateDefaultCVars()
        {
            /** CVar template Create<>("", , CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD); **/
            /** GENERAL **/
            Create<float>("tick_frequency", 120, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("initial_window_width", 1280, CVarFlags.PRESERVE);
            Create<int>("initial_window_height", 720, CVarFlags.PRESERVE);

            // "Screen" is the internal units of the game.
            // Always use `screen_width` and `screen_height`.
            Create<float>("screen_width", 1280, CVarFlags.DEV_PRESERVE);
            Create<float>("screen_height", 720, CVarFlags.DEV_PRESERVE);

            Create<float>("play_field_width", 1920, CVarFlags.DEV_PRESERVE);
            Create<float>("play_field_height", 1080, CVarFlags.DEV_PRESERVE);

            Create<float>("camera_padding", 200.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

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

            Create<bool>("display_vsync", true, CVarFlags.PRESERVE);

            Create<int>("display_fullscreen_width", -1, CVarFlags.PRESERVE);
            Create<int>("display_fullscreen_height", -1, CVarFlags.PRESERVE);

            /** UI logic **/
            Create<bool>("ui_mouse_mode", true, CVarFlags.LIVE_RELOAD);
            Create<int>("ui_gamepad_mode_current_operator", 0, CVarFlags.LIVE_RELOAD);
            Create<bool>("ui_auto_control_mode_switching", true, CVarFlags.LIVE_RELOAD);

            /** INPUT **/
            Create<float>("controller_deadzone", 0.1f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("controller_thumbstick", 1, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            // P1
            Create<int>("controller_0_rotate_left", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("controller_0_rotate_right", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            // P2
            Create<int>("controller_1_rotate_left", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("controller_1_rotate_right", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            // P3
            Create<int>("controller_2_rotate_left", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("controller_2_rotate_right", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            // P4
            Create<int>("controller_3_rotate_left", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("controller_3_rotate_right", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_pause", 16, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("input_shield_angular_speed", 5.026f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_primary_counter_clockwise", 65, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_keyboard_primary_clockwise", 68, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_secondary_counter_clockwise", 37, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_keyboard_secondary_clockwise", 39, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_pause", 27, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            /** GAMEPLAY **/
            Create<float>("game_speed", 1.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("game_difficulty", 1, CVarFlags.PRESERVE);

            Create<float>("player_shield_radius", 30.0f, CVarFlags.DEV_PRESERVE);
            Create<float>("player_shield_size", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("player_ship_size", 5, CVarFlags.DEV_PRESERVE);
            Create<int>("player_ship_max_health", 3, CVarFlags.DEV_PRESERVE);

            Create<bool>("god", false, CVarFlags.DEV_PRESERVE);

            Create<float>("enemy_pushback_force", 120.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("chasing_enemy_speed", 100.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("chasing_enemy_rotational_speed", 1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("chasing_enemy_size", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("chasing_enemy_acceleration", 13f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("shooting_enemy_size", 4, CVarFlags.DEV_PRESERVE);
            Create<float>("shooting_enemy_speed", 0, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("shooting_enemy_rotational_speed", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("shooting_enemy_projectile_speed", 160.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("shooting_enemy_projectile_bounces", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("shooting_enemy_projectile_ammo", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("gravity_enemy_size", 2, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_enemy_radius", 150.0f, CVarFlags.DEV_PRESERVE);
            Create<int>("gravity_hole_enemy_lifespan", 15, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_enemy_force", 230.0f, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_animation_rotation_speed", -0.8f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_size_multiplier_min", 0.8f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_size_multiplier_max", 1.2f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_size_period", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_spawn_duration", 1.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_despawn_duration", 0.9f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_ping_duration", 2f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("gravity_hole_animation_ping_period", 1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("projectile_size", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_chasing_enemy_initial_period", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_chasing_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_chasing_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

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

            Create<int>("game_over_responsible_enemy_flash_count", 10, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("game_over_responsible_enemy_flash_period", 0.16f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("game_over_edge_fade_out_duration", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("game_over_camera_reset_duration", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("game_over_ui_fade_in_duration", 0.7f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("background_stars_scale", 1.2f, CVarFlags.DEV_PRESERVE);

            Create<int>("score_base_destroy_enemy", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("score_base_destroy_enemy_with_projectile", 10, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("score_base_destroy_enemy_with_laser", 5, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            /** ANIMATION **/
            Create<float>("animation_spawn_warp_time_scale", 0.85f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("animation_spawn_warp_distance", 450, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("animation_spawn_warp_phase_1_base_duration", 0.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("animation_spawn_warp_phase_2_base_duration", 0.15f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

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
            Create<Color>("color_chasing_enemy", Color.Violet, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_shooting_enemy", Color.Cyan, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_laser_enemy", Color.Gold, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_gravity_hole_enemy", new Color(164, 4, 255), CVarFlags.DEV_PRESERVE);

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
            Create<bool>("debug_show_collision_shapes", false, CVarFlags.LIVE_RELOAD);
            Create<float>("debug_camera_zoom", 1, CVarFlags.LIVE_RELOAD);

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
            Create<string>("texture_chasing_enemy", "textures/Kamikaze", CVarFlags.PRESERVE);
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
            Create<string>("texture_ui_dropdown_background", "textures/ui/dropdown_contents_background", CVarFlags.PRESERVE);

            /** BEGIN KEYBOARD INPUT TEXTURES **/
            Create<string>("texture_input_keyboard_key_0", "textures/input/Keyboard/Keyboard_Black_0", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_1", "textures/input/Keyboard/Keyboard_Black_1", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_2", "textures/input/Keyboard/Keyboard_Black_2", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_3", "textures/input/Keyboard/Keyboard_Black_3", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_4", "textures/input/Keyboard/Keyboard_Black_4", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_5", "textures/input/Keyboard/Keyboard_Black_5", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_6", "textures/input/Keyboard/Keyboard_Black_6", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_7", "textures/input/Keyboard/Keyboard_Black_7", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_8", "textures/input/Keyboard/Keyboard_Black_8", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_9", "textures/input/Keyboard/Keyboard_Black_9", CVarFlags.PRESERVE);

            Create<string>("texture_input_keyboard_key_a", "textures/input/Keyboard/Keyboard_Black_A", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_b", "textures/input/Keyboard/Keyboard_Black_B", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_c", "textures/input/Keyboard/Keyboard_Black_C", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_d", "textures/input/Keyboard/Keyboard_Black_D", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_e", "textures/input/Keyboard/Keyboard_Black_E", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_f", "textures/input/Keyboard/Keyboard_Black_F", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_g", "textures/input/Keyboard/Keyboard_Black_G", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_h", "textures/input/Keyboard/Keyboard_Black_H", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_i", "textures/input/Keyboard/Keyboard_Black_I", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_j", "textures/input/Keyboard/Keyboard_Black_J", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_k", "textures/input/Keyboard/Keyboard_Black_K", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_l", "textures/input/Keyboard/Keyboard_Black_L", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_m", "textures/input/Keyboard/Keyboard_Black_M", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_n", "textures/input/Keyboard/Keyboard_Black_N", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_o", "textures/input/Keyboard/Keyboard_Black_O", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_p", "textures/input/Keyboard/Keyboard_Black_P", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_q", "textures/input/Keyboard/Keyboard_Black_Q", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_r", "textures/input/Keyboard/Keyboard_Black_R", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_s", "textures/input/Keyboard/Keyboard_Black_S", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_t", "textures/input/Keyboard/Keyboard_Black_T", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_u", "textures/input/Keyboard/Keyboard_Black_U", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_v", "textures/input/Keyboard/Keyboard_Black_V", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_w", "textures/input/Keyboard/Keyboard_Black_W", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_x", "textures/input/Keyboard/Keyboard_Black_X", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_y", "textures/input/Keyboard/Keyboard_Black_Y", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_z", "textures/input/Keyboard/Keyboard_Black_Z", CVarFlags.PRESERVE);

            Create<string>("texture_input_keyboard_key_up", "textures/input/Keyboard/Keyboard_Black_Arrow_Up", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_down", "textures/input/Keyboard/Keyboard_Black_Arrow_Down", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_left", "textures/input/Keyboard/Keyboard_Black_Arrow_Left", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_right", "textures/input/Keyboard/Keyboard_Black_Arrow_Right", CVarFlags.PRESERVE);

            Create<string>("texture_input_keyboard_key_bracket_left", "textures/input/Keyboard/Keyboard_Black_Bracket_Left", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_bracket_right", "textures/input/Keyboard/Keyboard_Black_Bracket_Right", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_semicolon", "textures/input/Keyboard/Keyboard_Black_Semicolon", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_quote", "textures/input/Keyboard/Keyboard_Black_Quote", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_angle_left", "textures/input/Keyboard/Keyboard_Black_Mark_Left", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_angle_right", "textures/input/Keyboard/Keyboard_Black_Mark_Right", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_question", "textures/input/Keyboard/Keyboard_Black_Question", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_slash", "textures/input/Keyboard/Keyboard_Black_Slash", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_minus", "textures/input/Keyboard/Keyboard_Black_Minus", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_plus", "textures/input/Keyboard/Keyboard_Black_Plus", CVarFlags.PRESERVE);
            Create<string>("texture_input_keyboard_key_space", "textures/input/Keyboard/Keyboard_Black_Space", CVarFlags.PRESERVE);

            Create<string>("texture_input_keyboard_key_blank", "textures/input/Keyboard/Blank_Black_Normal", CVarFlags.PRESERVE);
            /** END KEYBOARD INPUT TEXTURES **/

            /** BEGIN GAMEPAD INPUT TEXTURES **/
            Create<string>("texture_input_gamepad_a", "textures/input/XboxOne/XboxOne_A", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_b", "textures/input/XboxOne/XboxOne_B", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_x", "textures/input/XboxOne/XboxOne_X", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_y", "textures/input/XboxOne/XboxOne_Y", CVarFlags.PRESERVE);

            Create<string>("texture_input_gamepad_dpad_up", "textures/input/XboxOne/XboxOne_Dpad_Up", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_dpad_down", "textures/input/XboxOne/XboxOne_Dpad_Down", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_dpad_left", "textures/input/XboxOne/XboxOne_Dpad_Left", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_dpad_right", "textures/input/XboxOne/XboxOne_Dpad_Right", CVarFlags.PRESERVE);

            Create<string>("texture_input_gamepad_bumper_left", "textures/input/XboxOne/XboxOne_LB", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_bumper_right", "textures/input/XboxOne/XboxOne_RB", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_trigger_left", "textures/input/XboxOne/XboxOne_LT", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_trigger_right", "textures/input/XboxOne/XboxOne_RT", CVarFlags.PRESERVE);

            Create<string>("texture_input_gamepad_stick_left", "textures/input/XboxOne/XboxOne_Left_Stick", CVarFlags.PRESERVE);
            Create<string>("texture_input_gamepad_stick_right", "textures/input/XboxOne/XboxOne_Right_Stick", CVarFlags.PRESERVE);
            /** END GAMEPAD INPUT TEXTURES **/

            Create<float>("sound_master_volume", 1.0f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("sound_music_volume", 1.0f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("sound_effect_volume", 1.0f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<string>("sound_explosion", "sounds/Explosion_Sound", CVarFlags.PRESERVE);
            Create<string>("sound_projectile_fired", "sounds/Laser_Shot", CVarFlags.PRESERVE);
            Create<string>("sound_projectile_bounce", "sounds/Proj_Bounce", CVarFlags.PRESERVE);

            Create<string>("font_game_over", "fonts/Intro", CVarFlags.PRESERVE);
            Create<string>("font_texture_game_over", "fonts/Intro_0", CVarFlags.PRESERVE);
            Create<string>("font_hyperspace", "fonts/Hyperspace", CVarFlags.PRESERVE);
            Create<string>("font_texture_hyperspace", "fonts/Hyperspace_0", CVarFlags.PRESERVE);
            Create<string>("font_title", "fonts/Title", CVarFlags.PRESERVE);
            Create<string>("font_texture_title", "fonts/Title_0", CVarFlags.PRESERVE);

            Create<string>("effect_blur", "effects/Blur", CVarFlags.PRESERVE);
            Create<string>("effect_fxaa", "effects/FXAA", CVarFlags.PRESERVE);

            Create<string>("effect_field_font", "effects/FieldFontEffect", CVarFlags.PRESERVE);
            Create<string>("font_msdf_hyperspace", "fonts/Hyperspace/Hyperspace", CVarFlags.PRESERVE);

            Create<string>("ui_main_menu", "ui/MainMenu", CVarFlags.PRESERVE);
            Create<string>("ui_options_menu", "ui/OptionsMenu", CVarFlags.PRESERVE);
            Create<string>("ui_pause_menu", "ui/PauseMenu", CVarFlags.PRESERVE);
            Create<string>("ui_adrift_game_ui", "ui/MainGameStateUI", CVarFlags.PRESERVE);

            Create<string>("ui_lobby_menu", "ui/Lobby/LobbyMenu", CVarFlags.PRESERVE);
            Create<string>("ui_lobby_menu_join_instructions", "ui/Lobby/JoinInstructions", CVarFlags.PRESERVE);

            Create<string>("ui_game_over", "ui/GameOverScoreUI", CVarFlags.PRESERVE);

            Create<string>("ui_test", "ui/test", CVarFlags.PRESERVE);
            Create<string>("ui_test2", "ui/test2", CVarFlags.PRESERVE);
        }
    }
}
