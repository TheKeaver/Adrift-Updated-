namespace GameJam
{
    static partial class CVars
    {
        private static void CreateDefaultCVars()
        {
            /** GENERAL **/
            Create<float>("tick_frequency", 120);

            Create<int>("window_width", 960);
            Create<int>("window_height", 600);
            Create<float>("window_initial_aspect_ratio", (float)(CVars.Get<int>("window_width") / CVars.Get<int>("window_height")));

            Create<int>("screen_width", 1280);
            Create<int>("screen_height", 800);

            /** INPUT **/
            Create<float>("controller_deadzone", 0.1f);
            Create<int>("controller_thumbstick", 1);

            Create<float>("keyboard_shield_angular_speed", 2.513f);
            Create<int>("keyboard_primary_counter_clockwise", 65);
            Create<int>("keyboard_primary_clockwise", 68);

            /** GAMEPLAY **/
            Create<float>("player_shield_radius", 30.0f);
            Create<int>("player_ship_max_health", 3);

            Create<float>("kamikaze_enemy_speed", 50.0f);
            Create<float>("kamikaze_enemy_rotational_speed", 0.5f);
            Create<float>("kamikaze_enemy_pushback_force", 6);

            Create<float>("shooting_enemy_speed", 0);
            Create<float>("shooting_enemy_rotational_speed", 1.5f);
            Create<float>("shooting_enemy_projectile_speed", 60.0f);
            Create<int>("shooting_enemy_projectile_bounces", 3);
            Create<int>("shooting_enemy_projectile_ammo", 4);

            Create<float>("spawner_kamikaze_enemy_initial_period", 3);
            Create<float>("spawner_kamikaze_enemy_period_multiplier", 0.99f);
            Create<float>("spawner_kamikaze_enemy_period_min", 1);

            Create<float>("spawner_shooting_enemy_initial_period", 10);
            Create<float>("spawner_shooting_enemy_period_multiplier", 0.99f);
            Create<float>("spawner_shooting_enemy_period_min", 1);

            Create<float>("spawner_min_distance_away_from_player", 200.0f);
            Create<int>("spawner_max_enemy_count", 50);

            /** RESOURCES **/
            Create<string>("texture_player_ship", "textures/PlayerShip");
            Create<string>("texture_player_shield", "textures/shield");
            Create<string>("texture_explosion", "textures/Explosion");
            Create<string>("texture_kamikaze", "textures/Kamikaze");
            Create<string>("texture_shooter_enemy", "textures/ShootingEnemy");
            Create<string>("texture_enemy_bullet", "textures/EnemyBullet");

            Create<string>("texture_title_with_instructions", "textures/TitleInstructions");
            Create<string>("texture_title_without_instructions", "textures/TitleNoInstructions");

            Create<string>("sound_explosion", "sounds/Explosion_Sound");
            Create<string>("sound_projectile_fired", "sounds/Laser_Shot");
            Create<string>("sound_projectile_bounce", "sounds/Proj_Bounce");

            Create<string>("font_game_over", "fonts/Intro");
        }
    }
}
