using Microsoft.Xna.Framework;

namespace GameJam
{
    static partial class CVars
    {
        private static void CreateDefaultCVars()
        {
            /** GENERAL **/
            Create<float>("tick_frequency", 120, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("window_width", 960, CVarFlags.PRESERVE);
            Create<int>("window_height", 600, CVarFlags.PRESERVE);
            Create<float>("window_initial_aspect_ratio", (float)CVars.Get<int>("window_width") / CVars.Get<int>("window_height"), CVarFlags.PRESERVE);

            Create<int>("screen_width", 1280);
            Create<int>("screen_height", 800);

            /** GRAPHICS **/
            Create<bool>("graphics_fxaa", false, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<bool>("graphics_frame_smoothing", true, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            /** INPUT **/
            Create<float>("input_controller_deadzone", 0.1f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_thumbstick", 1, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_counter_clockwise", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_controller_clockwise", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("input_shield_angular_speed", 2.513f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_primary_counter_clockwise", 65, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_keyboard_primary_clockwise", 68, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("input_keyboard_secondary_counter_clockwise", 37, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("input_keyboard_secondary_clockwise", 39, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            /** GAMEPLAY **/
            Create<float>("player_shield_radius", 30.0f, CVarFlags.DEV_PRESERVE);
            Create<float>("player_shield_size", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("player_ship_size", 5, CVarFlags.DEV_PRESERVE);
            Create<int>("player_ship_max_health", 3, CVarFlags.DEV_PRESERVE);

            Create<float>("kamikaze_enemy_speed", 50.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("kamikaze_enemy_rotational_speed", 0.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("kamikaze_size", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("enemy_pushback_force", 60.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("shooting_enemy_size", 4, CVarFlags.DEV_PRESERVE);
            Create<float>("shooting_enemy_speed", 0, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("shooting_enemy_rotational_speed", 1.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("shooting_enemy_projectile_speed", 60.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("shooting_enemy_projectile_bounces", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("shooting_enemy_projectile_ammo", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("gravity_enemy_size", 2, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_enemy_radius", 125.0f, CVarFlags.DEV_PRESERVE);
            Create<int>("gravity_hole_enemy_lifespan", 15, CVarFlags.DEV_PRESERVE);
            Create<float>("gravity_hole_enemy_force", 60.0f, CVarFlags.DEV_PRESERVE);

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

            Create<float>("laser_enemy_rotational_speed", 1.5f, CVarFlags.DEV_PRESERVE);
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

            /** COLORS **/
            Create<Color>("color_player_ship", Color.White, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_player_shield", Color.SpringGreen, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_kamikaze_enemy", Color.Violet, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_shooting_enemy", Color.Cyan, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_laser_enemy", Color.Gold, CVarFlags.DEV_PRESERVE);

            Create<Color>("color_projectile", Color.Red, CVarFlags.DEV_PRESERVE);
            Create<Color>("color_laser_beam", Color.Red, CVarFlags.DEV_PRESERVE);

            Create<Color>("color_playfield", Color.Green, CVarFlags.DEV_PRESERVE);

            /** DEBUG **/
            Create<bool>("debug_show_cvar_viewer", false, CVarFlags.LIVE_RELOAD);

            /** PARTICLES **/
            Create<int>("particle_explosion_count", 150, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_strength", 1000, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_variety_min", 0.1f, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_variety_max", 1f, CVarFlags.DEV_PRESERVE);
            Create<float>("particle_explosion_duration", 150, CVarFlags.DEV_PRESERVE);

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

            Create<string>("effect_blur", "effects/Blur", CVarFlags.PRESERVE);
            Create<string>("effect_fxaa", "effects/FXAA", CVarFlags.PRESERVE);
        }
    }
}
