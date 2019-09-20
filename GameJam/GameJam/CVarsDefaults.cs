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

            /** INPUT **/
            Create<float>("controller_deadzone", 0.1f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("controller_thumbstick", 1, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("keyboard_shield_angular_speed", 2.513f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("keyboard_primary_counter_clockwise", 65, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("keyboard_primary_clockwise", 68, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<int>("keyboard_secondary_counter_clockwise", 37, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("keyboard_secondary_clockwise", 39, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD);

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

            Create<float>("projectile_size", 2, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_kamikaze_enemy_initial_period", 3, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_kamikaze_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_kamikaze_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_shooting_enemy_initial_period", 10, CVarFlags.DEV_PRESERVE);
            Create<float>("spawner_shooting_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<float>("spawner_shooting_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            Create<float>("spawner_min_distance_away_from_player", 200.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);
            Create<int>("spawner_max_enemy_count", 50, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD);

            /** RESOURCES **/
            Create<string>("texture_player_ship", "textures/PlayerShip", CVarFlags.PRESERVE);
            Create<string>("texture_player_shield", "textures/shield", CVarFlags.PRESERVE);
            Create<string>("texture_explosion", "textures/Explosion", CVarFlags.PRESERVE);
            Create<string>("texture_kamikaze", "textures/Kamikaze", CVarFlags.PRESERVE);
            Create<string>("texture_shooter_enemy", "textures/ShootingEnemy", CVarFlags.PRESERVE);
            Create<string>("texture_enemy_bullet", "textures/EnemyBullet", CVarFlags.PRESERVE);

            Create<string>("texture_title_with_instructions", "textures/TitleInstructions", CVarFlags.PRESERVE);
            Create<string>("texture_title_without_instructions", "textures/TitleNoInstructions", CVarFlags.PRESERVE);

            Create<string>("texture_ui_button_released", "textures/ui/button_up_background", CVarFlags.PRESERVE);
            Create<string>("texture_ui_button_hover", "textures/ui/button_over_background", CVarFlags.PRESERVE);
            Create<string>("texture_ui_button_pressed", "textures/ui/button_down_background", CVarFlags.PRESERVE);

            Create<string>("sound_explosion", "sounds/Explosion_Sound", CVarFlags.PRESERVE);
            Create<string>("sound_projectile_fired", "sounds/Laser_Shot", CVarFlags.PRESERVE);
            Create<string>("sound_projectile_bounce", "sounds/Proj_Bounce", CVarFlags.PRESERVE);

            Create<string>("font_game_over", "fonts/Intro", CVarFlags.PRESERVE);
        }
    }
}
