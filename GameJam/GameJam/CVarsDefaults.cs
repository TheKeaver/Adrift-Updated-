using Microsoft.Xna.Framework;

namespace GameJam
{
    static partial class CVars
    {
        private static void CreateDefaultCVars()
        {
            /** CVar template Create<>("", , CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD); **/
            /** GENERAL **/
            Create<string>("game_version", "v0.1-SNAPSHOT", 0, "Version of the game.");

            Create<float>("tick_frequency", 120, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Frequency of game ticks (updates).");
            Create<bool>("update_xna_fixed", false, CVarFlags.PRESERVE, "Update with XNA fixed updates (60Hz update calls).");

            Create<int>("initial_window_width", 1280, CVarFlags.PRESERVE, "Initial width of the window when the game is launched.");
            Create<int>("initial_window_height", 720, CVarFlags.PRESERVE, "Initial height of the window when the game is launched.");

            // "Screen" is the internal units of the game.
            // Always use `screen_width` and `screen_height`.
            Create<float>("screen_width", 1280, CVarFlags.DEV_PRESERVE, "Virtual width of the screen.");
            Create<float>("screen_height", 720, CVarFlags.DEV_PRESERVE, "Virtual height of the screen.");

            Create<float>("play_field_width", 1920, CVarFlags.DEV_PRESERVE, "Width of the playfield.");
            Create<float>("play_field_height", 1080, CVarFlags.DEV_PRESERVE, "Height of the playfield");

            Create<float>("camera_padding", 200.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Additional padding between the edge of the camera and the players during camera following behavior.");
            Create<float>("camera_tracking_speed", 0.05f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "A multiplier that affects how much the camera can move every update. Larger number means more movement per update");

            Create<int>("quad_tree_max_references", 4, CVarFlags.DEV_PRESERVE, "Maximum references in a quad-tree node before quad-tree node splits.");

            /** GRAPHICS **/
            Create<bool>("graphics_fxaa", false, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Enable FXAA anti-aliasing.");
            Create<bool>("graphics_smaa", false, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Enable SMAA anti-aliasing.");
            Create<bool>("graphics_frame_smoothing", true, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Enable frame smoothing.");
            Create<bool>("graphics_feathering", true, CVarFlags.PRESERVE, "Enable feathering anti-aliasing.");
            Create<float>("graphics_feathering_width", 0.2f, CVarFlags.PRESERVE, "Width of feathering.");
            /**
             * If more than one are true; priority in order of highest
             * to lowest: fullscreen, borderless, windowed
             * **/
            Create<bool>("display_windowed", true, CVarFlags.PRESERVE, "Enable windowed mode (`display_borderless` takes precedent).");
            Create<bool>("display_borderless", false, CVarFlags.PRESERVE, "Enable borderless fullscreen mode (`display_fullscreen` takes precedent).");
            Create<bool>("display_fullscreen", false, CVarFlags.PRESERVE, "Enable fullscreen mode.");

            Create<bool>("display_vsync", true, CVarFlags.PRESERVE, "Enable vertical synchronization.");

            Create<int>("display_fullscreen_width", -1, CVarFlags.PRESERVE, "Fullscreen resolution width.");
            Create<int>("display_fullscreen_height", -1, CVarFlags.PRESERVE, "Fullscreen resolution height.");

            /** UI logic **/
            Create<bool>("ui_mouse_mode", true, CVarFlags.LIVE_RELOAD, "Indicates whether the UI is in mouse mode (gamepad mode if false).");
            Create<int>("ui_gamepad_mode_current_operator", 0, CVarFlags.LIVE_RELOAD, "Indicates which gamepad is currently operating the UI.");
            Create<bool>("ui_auto_control_mode_switching", true, CVarFlags.LIVE_RELOAD, "Enables automatically switching between mouse/gamepad mode.");

            /** INPUT **/
            Create<float>("controller_deadzone", 0.1f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Deadzone of the controllers");
            Create<int>("controller_thumbstick", 1, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Thumbstick of the controller to use.");
            // P1
            Create<int>("controller_0_rotate_counter_clockwise", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 0 rotate left button.");
            Create<int>("controller_0_rotate_clockwise", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 0 rotate right button.");
            Create<int>("controller_0_super_shield", 16384, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 0 activate super shield 'X' button");
            // P2        
            Create<int>("input_controller_1_rotate_counter_clockwise", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 1 rotate left button.");
            Create<int>("input_controller_1_rotate_clockwise", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 1 rotate right button.");
            Create<int>("input_controller_1_super_shield", 16384, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 1 activate super shield 'X' button");
            // P3        
            Create<int>("input_controller_2_rotate_counter_clockwise", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 2 rotate left button.");
            Create<int>("input_controller_2_rotate_clockwise", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 2 rotate right button.");
            Create<int>("input_controller_2_super_shield", 16384, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 2 activate super shield 'X' button");
            // P4        
            Create<int>("input_controller_3_rotate_counter_clockwise", 256, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 3 rotate left button.");
            Create<int>("input_controller_3_rotate_clockwise", 512, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 3 rotate right button.");
            Create<int>("input_controller_3_super_shield", 16384, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Controller 3 activate super shield 'X' button");
            Create<int>("input_controller_pause", 16, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Pause button of the controller.");

            Create<float>("input_shield_angular_speed", 5.026f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Angular speed of the speed rotation.");

            Create<int>("input_keyboard_primary_rotate_counter_clockwise", 65, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Primary keyboard rotate left key. 'A' key");
            Create<int>("input_keyboard_primary_rotate_clockwise", 68, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Primary keyboard rotate right key. 'D' key");
            Create<int>("input_keyboard_primary_super_shield", 83, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Primary keyboard super shield. 'S' key");

            Create<int>("input_keyboard_secondary_rotate_counter_clockwise", 37, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Secondary keyboard rotate left arrow key.");
            Create<int>("input_keyboard_secondary_rotate_clockwise", 39, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Secondary keyboard rotate right arrow key.");
            Create<int>("input_keyboard_secondary_super_shield", 40, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Secondary keyboard super shield down arrow key");

            Create<int>("input_keyboard_pause", 27, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Pause key of the keyboard.");

            /** GAMEPLAY **/
            Create<float>("game_speed", 1.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Game speed.");
            Create<int>("game_difficulty", 1, CVarFlags.PRESERVE, "Game difficulty.");

            Create<float>("player_shield_radius", 30.0f, CVarFlags.DEV_PRESERVE, "Radius of the player shield away from the ship");
            Create<float>("player_shield_size", 3, CVarFlags.DEV_PRESERVE, "Size of the player shield.");
            Create<float>("player_ship_size", 5, CVarFlags.DEV_PRESERVE, "Size of the player ship.");
            Create<int>("player_ship_max_health", 3, CVarFlags.DEV_PRESERVE, "Base health of the player ship.");
            Create<float>("player_ship_multiplayer_spawn_radius", 35.0f, CVarFlags.DEV_PRESERVE, "Radius from the origin to spawn the players around when in multiplayer (>1 player).");
            Create<float>("player_super_shield_max", 5, CVarFlags.LIVE_RELOAD, "CHANGEME");
            Create<float>("player_super_shield_spend_rate", 2, CVarFlags.LIVE_RELOAD, "CHANGEME");
            Create<float>("player_super_shield_regen_rate", 1, CVarFlags.LIVE_RELOAD, "CHANGEME");

            Create<bool>("player_individual_deaths", true, CVarFlags.LIVE_RELOAD | CVarFlags.DEV_PRESERVE, "Whether players will have shared or individual deaths.");

            Create<bool>("god", false, CVarFlags.DEV_PRESERVE, "Enable god mode.");

            Create<float>("enemy_pushback_force", 120.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Pushback force when an enemy is destroyed by a player shield.");
            Create<float>("enemy_minimum_separation_distance", 5, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum distance enemies must be separated from each other");

            Create<float>("chasing_enemy_speed", 100.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Speed of chasing enemies.");
            Create<float>("chasing_enemy_rotational_speed", 1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Rotational speed of chasing enemies.");
            Create<float>("chasing_enemy_size", 3, CVarFlags.DEV_PRESERVE, "Size of chasing enemies.");
            Create<float>("chasing_enemy_acceleration", 13f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Acceleration of chasing enemies.");

            Create<float>("shooting_enemy_size", 4, CVarFlags.DEV_PRESERVE, "Size of shooting enemies.");
            Create<float>("shooting_enemy_speed", 0, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Speed of shooting enemies.");
            Create<float>("shooting_enemy_rotational_speed", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Rotational speed of shooting enemies.");
            Create<float>("shooting_enemy_projectile_speed", 160.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Speed of projectiles launched by shooting enemies.");
            Create<int>("shooting_enemy_projectile_bounces", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Max bounces shooting enemy projectiles have before destruction.");
            Create<int>("shooting_enemy_projectile_ammo", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Amount of ammo shooting enemies have before transforming into chasing enemies.");

            Create<float>("gravity_enemy_size", 2, CVarFlags.DEV_PRESERVE, "Size of gravity holes.");
            Create<float>("gravity_hole_enemy_radius", 150.0f, CVarFlags.DEV_PRESERVE, "Radius of gravity holes.");
            Create<int>("gravity_hole_enemy_lifespan", 15, CVarFlags.DEV_PRESERVE, "Lifespand of gravity holes in seconds.");
            Create<float>("gravity_hole_enemy_force", 230.0f, CVarFlags.DEV_PRESERVE, "Force gravity holes place on gameplay entities.");
            Create<float>("gravity_hole_animation_rotation_speed", -0.8f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Speed of gravity hole rotation animation.");
            Create<float>("gravity_hole_animation_size_multiplier_min", 0.8f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum size of gravity hole pulse animation.");
            Create<float>("gravity_hole_animation_size_multiplier_max", 1.2f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Maximum size of gravity hole pulse animation.");
            Create<float>("gravity_hole_animation_size_period", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Period (in seconds) of gravity hole pulse animation.");
            Create<float>("gravity_hole_animation_spawn_duration", 1.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of gravity hole spawn animation.");
            Create<float>("gravity_hole_animation_despawn_duration", 0.9f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of gravity hole despawn animation.");
            Create<float>("gravity_hole_animation_ping_duration", 2f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of gravity hole ping animation.");
            Create<float>("gravity_hole_animation_ping_period", 1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Period of gravity hole ping animation.");

            Create<float>("projectile_size", 4, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Size of enemy projectiles.");

            Create<float>("spawner_chasing_enemy_initial_period", 3, CVarFlags.DEV_PRESERVE, "Chasing enemy spawner initial period.");
            Create<float>("spawner_chasing_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Chasing enemy spawner period multiplier.");
            Create<float>("spawner_chasing_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum period of chasing enemy spawner.");

            Create<float>("spawner_shooting_enemy_initial_period", 10, CVarFlags.DEV_PRESERVE, "Shooting enemy spawner initial period.");
            Create<float>("spawner_shooting_enemy_period_multiplier", 0.99f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Shooting enemy spawner period multiplier.");
            Create<float>("spawner_shooting_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum period of shooting enemy spawner period.");

            Create<float>("spawner_gravity_enemy_initial_period", 15, CVarFlags.DEV_PRESERVE, "Gravity hole spawner initial period.");
            Create<float>("spawner_gravity_enemy_period_multiplier", 0.999f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Gravity hole spawner period multiplier.");
            Create<float>("spawner_gravity_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum period of gravity hole spawner.");

            Create<float>("spawner_laser_enemy_initial_period", 15, CVarFlags.DEV_PRESERVE, "Laser enemy spawner initial period.");
            Create<float>("spawner_laser_enemy_period_multiplier", 0.9f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Laser enemy spawner period multiplier.");
            Create<float>("spawner_laser_enemy_period_min", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum period of laser enemy spawner.");
            Create<int>("spawner_laser_enemy_max_entities", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Maximum laser enemies the laser enemy spawner can spawn.");

            Create<float>("spawner_min_distance_away_from_player", 200.0f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Minimum distance enemies must spawn away from the player.");
            Create<int>("spawner_max_enemy_count", 50, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Maximum enemies that may be spawned.");

            Create<float>("laser_enemy_rotational_speed", 3f, CVarFlags.DEV_PRESERVE, "Rotational speed of laser enemies.");
            Create<float>("laser_enemy_size", 4f, CVarFlags.DEV_PRESERVE, "Size of laser enemies.");
            Create<float>("laser_enemy_warm_up_anim_duration", 0.1f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of laser warm up animation for laser enemies.");
            Create<float>("laser_enemy_warm_up_duration", 2, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of laser warm up before shooting for laser enemies.");
            Create<float>("laser_enemy_warm_up_thickness", 1.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Thickness of laser warm up beam for laser enemies.");
            Create<float>("laser_enemy_fire_duration", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Laser enemy fire duration.");
            Create<float>("laser_enemy_fire_frequency", 80, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Frequency of pulsing laser during laser fire from laser enemy.");
            Create<float>("laser_enemy_fire_thickness", 5, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Thickness of laser during laser fire from laser enemy.");
            Create<float>("laser_enemy_fire_thickness_variability", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Plus or minus veriability from maximum thickness during laser fire from laser enemy.");
            Create<float>("laser_enemy_fire_initial_thickness_decay_factor", 25, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Ramp up inverse exponential decay factor from warm up beam to full beam for laser enemies.");
            Create<float>("laser_enemy_fire_closing_envelope_decay_factor", 20, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Ramp down exponential decay factor from full beam to zero beam for laser enemies.");
            Create<float>("laser_enemy_spawn_wait_period", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Period to wait between laser enemy spawning and the first laser beam warming up.");
            Create<float>("laser_enemy_successive_wait_period", 7, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Period to wait after laser beam fire to the next laser beam warming up.");

            Create<int>("game_over_responsible_enemy_flash_count", 10, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Number of flashes of enemy responsible for game over.");
            Create<float>("game_over_responsible_enemy_flash_period", 0.16f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Period between enemy flashes for enemy responsible for game over.");
            Create<float>("game_over_edge_fade_out_duration", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of edge fade out animation for game over.");
            Create<float>("game_over_camera_reset_duration", 3, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of camera reset animation to origin for game over.");
            Create<float>("game_over_ui_fade_in_duration", 0.7f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Duration of UI fade in animation for game over gamestate.");

            Create<float>("background_stars_scale", 1.2f, CVarFlags.DEV_PRESERVE, "Scale of background stars.");

            Create<int>("score_base_destroy_enemy", 1, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Base score of destroying an enemy with a shield.");
            Create<int>("score_base_destroy_enemy_with_projectile", 10, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Base score of destroying an enemy with a projectile.");
            Create<int>("score_base_destroy_enemy_with_laser", 5, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Base score of destroyting an enemy with a laser.");

            /** ANIMATION **/
            Create<float>("animation_spawn_warp_time_scale", 0.85f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Enemy warp-in animation time scale.");
            Create<float>("animation_spawn_warp_distance", 450, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Enemy warp-in animation travel distance.");
            Create<float>("animation_spawn_warp_phase_1_base_duration", 0.5f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Enemy warp-in animation phase 1 animation duration.");
            Create<float>("animation_spawn_warp_phase_2_base_duration", 0.15f, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Enemy warp-in animation phase 2 animation duration.");

            /** MENU **/
            Create<float>("entity_background_spawner_min", 1f, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner minimum period.");
            Create<float>("entity_background_spawner_max", 2, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner maximum period.");
            Create<float>("entity_background_spawner_x", CVars.Get<float>("screen_width") / 2 * 1.05f, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner position of spawned enemies.");
            Create<float>("entity_background_spawner_y_min", CVars.Get<float>("screen_height") * -0.49f, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner minimum y position of spawned enemies.");
            Create<float>("entity_background_spawner_y_max", CVars.Get<float>("screen_height") * 0.49f, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner maximum y position of spawned enemies.");
            Create<float>("entity_background_spawner_destruction_x", -CVars.Get<float>("entity_background_spawner_x"), CVarFlags.DEV_PRESERVE, "Menu background enemy spawner position of destruction of enemies.");
            Create<float>("entity_background_entity_speed_min", 50, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner minimum enemy travel speed.");
            Create<float>("entity_background_entity_speed_max", 175, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner maximum enemy travel speed.");
            Create<float>("entity_background_entity_angular_speed_min", -MathHelper.PiOver2, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner minimum angular speed.");
            Create<float>("entity_background_entity_angular_speed_max", MathHelper.PiOver2, CVarFlags.DEV_PRESERVE, "Menu background enemy spawner maximum angular speed.");

            /** COLORS **/
            Create<Color>("color_player_ship", Color.White, CVarFlags.DEV_PRESERVE, "Color of player ship.");
            Create<Color>("color_player_shield_high", Color.SpringGreen, CVarFlags.DEV_PRESERVE, "Color of player shield when at maximum health.");
            Create<Color>("color_player_shield_middle", Color.Yellow, CVarFlags.DEV_PRESERVE, "Color of player shield when at half health.");
            Create<Color>("color_player_shield_low", Color.Red, CVarFlags.DEV_PRESERVE, "Color of player shield when at minimum health (before game over).");
            Create<Color>("color_chasing_enemy", Color.Violet, CVarFlags.DEV_PRESERVE, "Color of chasing enemies.");
            Create<Color>("color_shooting_enemy", Color.Cyan, CVarFlags.DEV_PRESERVE, "Color of shooting enemies.");
            Create<Color>("color_laser_enemy", Color.Gold, CVarFlags.DEV_PRESERVE, "Color of laser enemies.");
            Create<Color>("color_gravity_hole_enemy", new Color(164, 4, 255), CVarFlags.DEV_PRESERVE, "Color of gravity hole enemies.");

            Create<Color>("color_projectile", Color.Red, CVarFlags.DEV_PRESERVE, "Color of enemy projectiles.");
            Create<Color>("color_projectile_friendly", new Color(10, 252, 87, 255), CVarFlags.DEV_PRESERVE, "Color of friendly projectiles.");
            Create<Color>("color_laser_beam", Color.Red, CVarFlags.DEV_PRESERVE, "Color of laser beams.");

            Create<Color>("color_playfield", Color.Green, CVarFlags.DEV_PRESERVE, "Color of playfield border.");

            /** DEBUG **/
            Create<bool>("debug_show_cvar_viewer", false, CVarFlags.LIVE_RELOAD, "Show CVar viewer.");
            Create<bool>("debug_show_playback_controls", false, CVarFlags.LIVE_RELOAD, "Show playback controls.");
            Create<bool>("debug_pause_game_updates", false, CVarFlags.LIVE_RELOAD, "Pause gameplay updates (rendering still active).");
            Create<float>("debug_update_time_scale", 1.0f, CVarFlags.LIVE_RELOAD, "Time scale of game speed.");
            Create<float>("debug_game_step_period", 1 / 60.0f, CVarFlags.LIVE_RELOAD, "Step period of game when game is paused and step button is pressed.");
            Create<bool>("debug_show_console", false, CVarFlags.LIVE_RELOAD, "Show developer console.");
            Create<int>("debug_max_console_entries", 1000, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Maximum entries in the developer console.");
            Create<string>("debug_console_filter", @"Mouse[A-z]+|Keyboard[A-z]+|Component[A-z]+|Entity[A-z]+|CollisionEnd[A-z]+", CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Regular express of events to filter out from displaying in the developer console.");
            Create<bool>("debug_show_statistics", false, CVarFlags.LIVE_RELOAD, "Show statistics window.");
            Create<int>("debug_statistics_average_between_ticks_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Sample size of average between ticks time.");
            Create<int>("debug_statistics_average_between_frames_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Sample size of average between frames time.");
            Create<int>("debug_statistics_average_update_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Sample size of average update time.");
            Create<int>("debug_statistics_average_draw_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Sample size of average draw time.");
            Create<int>("debug_statistics_average_particle_sample", 30, CVarFlags.DEV_PRESERVE | CVarFlags.LIVE_RELOAD, "Sample size of average particle count.");
            Create<bool>("debug_show_collision_shapes", false, CVarFlags.LIVE_RELOAD, "Show collision hulls and bounding boxes.");
            Create<bool>("debug_show_render_culling", false, CVarFlags.LIVE_RELOAD, "Show render culling bounding boxes.");
            Create<bool>("debug_show_quad_trees", false, CVarFlags.LIVE_RELOAD, "Show quad trees.");
            Create<bool>("debug_enable_camera_movement", true, CVarFlags.LIVE_RELOAD, "Enable camera movement behavior.");
            Create<float>("debug_gameplay_camera_zoom", 1, CVarFlags.LIVE_RELOAD, "Zoom in addition to gameplay camera zoom.");
            Create<bool>("debug_force_debug_camera", false, CVarFlags.LIVE_RELOAD, "Force using debug camera.");
            Create<float>("debug_debug_camera_zoom", 1, CVarFlags.LIVE_RELOAD, "Debug camera zoom.");
            Create<float>("debug_debug_camera_zoom_speed", 0.0001f, CVarFlags.LIVE_RELOAD, "Debug camera zoom speed (scroll wheel).");
            Create<float>("debug_debug_camera_position_x", 0, CVarFlags.LIVE_RELOAD, "Debug camera x position.");
            Create<float>("debug_debug_camera_position_y", 0, CVarFlags.LIVE_RELOAD, "Debug camera y position.");

            /** PARTICLES **/
            Create<bool>("particle_enable", true, CVarFlags.DEV_PRESERVE, "Enable particles.");
            Create<bool>("particle_gpu_accelerated", true, CVarFlags.PRESERVE, "Enable GPU accelerated particles.");
            Create<int>("particle_gpu_buffer_size", 1024, CVarFlags.DEV_PRESERVE, "Square root of GPU particle buffer size.");
            Create<float>("particle_explosion_scale_x", 22, CVarFlags.DEV_PRESERVE, "Explosion particle x scale.");
            Create<float>("particle_explosion_scale_y", 1, CVarFlags.DEV_PRESERVE, "Explosion particle y scale.");
            Create<int>("particle_explosion_count", 200, CVarFlags.DEV_PRESERVE, "Explosion particle count per explosion.");
            Create<float>("particle_explosion_strength", 1400.0f, CVarFlags.DEV_PRESERVE, "Strength of explosion particles.");
            Create<float>("particle_explosion_decay_multiplier", 0.96f, CVarFlags.DEV_PRESERVE, "Decay of explosion particles.");
            Create<float>("particle_explosion_variety_min", 0.1f, CVarFlags.DEV_PRESERVE, "Minimum variety in properties of explosion particles.");
            Create<float>("particle_explosion_variety_max", 1f, CVarFlags.DEV_PRESERVE, "Maximum variety in properties of explosion particles.");

            /** RESOURCES **/
            Create<string>("complete_texture_atlas", "textures/texture_atlas/CompleteTextureAtlas", 0, "Path of texture atlas map.");
            Create<string>("complete_texture_atlas_texture", "textures/texture_atlas/CompleteTextureAtlas_texture", 0, "Path of texture atlas texture.");

            Create<string>("texture_player_ship", "textures/PlayerShip", CVarFlags.PRESERVE, "Path of player ship texture.");
            Create<string>("texture_player_shield", "textures/shield", CVarFlags.PRESERVE, "Path of player shield texture.");
            Create<string>("texture_explosion", "textures/Explosion", CVarFlags.PRESERVE, "Path of explosion texture.");
            Create<string>("texture_chasing_enemy", "textures/Kamikaze", CVarFlags.PRESERVE, "Path of chasing enemy texture.");
            Create<string>("texture_shooter_enemy", "textures/ShootingEnemy", CVarFlags.PRESERVE, "Path of shooting enemy texture.");
            Create<string>("texture_enemy_bullet", "textures/EnemyBullet", CVarFlags.PRESERVE, "Path of enemy bullet texture.");

            Create<string>("texture_title_with_instructions", "textures/TitleInstructions", CVarFlags.PRESERVE, "Path of title with instructions texture.");
            Create<string>("texture_title_without_instructions", "textures/TitleNoInstructions", CVarFlags.PRESERVE, "Path of title without instructions texture.");

            Create<string>("texture_particle_velocity", "textures/particles/VelocityParticle", CVarFlags.PRESERVE, "Path of velocity particle texture.");
            Create<string>("effect_gpu_particle_velocity", "effects/VelocityGPUParticleEffect", CVarFlags.PRESERVE, "Path of GPU accelerated velocity particle update/draw shader.");

            Create<string>("texture_background_stars_0", "textures/Stars/000", CVarFlags.PRESERVE, "Path of background stars 0 texture.");
            Create<string>("texture_background_stars_1", "textures/Stars/001", CVarFlags.PRESERVE, "Path of background stars 1 texture.");
            Create<string>("texture_background_stars_2", "textures/Stars/002", CVarFlags.PRESERVE, "Path of background stars 2 texture.");
            Create<string>("texture_background_stars_3", "textures/Stars/003", CVarFlags.PRESERVE, "Path of background stars 3 texture.");
            Create<string>("texture_background_stars_4", "textures/Stars/004", CVarFlags.PRESERVE, "Path of background stars 4 texture.");
            Create<string>("texture_background_stars_5", "textures/Stars/005", CVarFlags.PRESERVE, "Path of background stars 5 texture.");
            Create<string>("texture_background_stars_6", "textures/Stars/006", CVarFlags.PRESERVE, "Path of background stars 6 texture.");
            Create<string>("texture_background_stars_7", "textures/Stars/007", CVarFlags.PRESERVE, "Path of background stars 7 texture.");
            Create<string>("texture_background_stars_8", "textures/Stars/008", CVarFlags.PRESERVE, "Path of background stars 8 texture.");
            Create<string>("texture_background_stars_9", "textures/Stars/009", CVarFlags.PRESERVE, "Path of background stars 9 texture.");
            Create<string>("texture_background_stars_10", "textures/Stars/010", CVarFlags.PRESERVE, "Path of background stars 10 texture.");
            Create<string>("texture_background_stars_11", "textures/Stars/011", CVarFlags.PRESERVE, "Path of background stars 11 texture.");
            Create<string>("texture_background_stars_12", "textures/Stars/012", CVarFlags.PRESERVE, "Path of background stars 12 texture.");
            Create<string>("texture_background_stars_13", "textures/Stars/013", CVarFlags.PRESERVE, "Path of background stars 13 texture.");

            Create<string>("texture_background_parallax_test", "textures/ParalaxTestBackground", CVarFlags.PRESERVE, "Path of parallax test background texture..");

            Create<float>("texture_gravity_hole_radius", 45, CVarFlags.LIVE_RELOAD, "Radius of gravity hole texture.");
            Create<string>("texture_gravity_hole_circle", "textures/GravityHoleCircle", CVarFlags.PRESERVE, "Path of gravity hole circle texture.");

            Create<string>("texture_ui_button_released", "textures/ui/button_up_background", CVarFlags.PRESERVE, "Path of UI button released texture.");
            Create<string>("texture_ui_button_hover", "textures/ui/button_over_background", CVarFlags.PRESERVE, "Path of UI button hover texture.");
            Create<string>("texture_ui_button_pressed", "textures/ui/button_down_background", CVarFlags.PRESERVE, "Path of UI button pressed texture.");
            Create<string>("texture_ui_lobby_panel_background", "textures/ui/LobbyPanelBackground", CVarFlags.PRESERVE, "Path of UI lobby panel background texture.");
            Create<string>("texture_ui_dropdown_background", "textures/ui/dropdown_contents_background", CVarFlags.PRESERVE, "Path of UI dropdown background texture.");

            /** BEGIN KEYBOARD INPUT TEXTURES **/
            Create<string>("texture_input_keyboard_key_0", "textures/input/Keyboard/Keyboard_Black_0", CVarFlags.PRESERVE, "Path of keyboard 0 texture.");
            Create<string>("texture_input_keyboard_key_1", "textures/input/Keyboard/Keyboard_Black_1", CVarFlags.PRESERVE, "Path of keyboard 1 texture.");
            Create<string>("texture_input_keyboard_key_2", "textures/input/Keyboard/Keyboard_Black_2", CVarFlags.PRESERVE, "Path of keyboard 2 texture.");
            Create<string>("texture_input_keyboard_key_3", "textures/input/Keyboard/Keyboard_Black_3", CVarFlags.PRESERVE, "Path of keyboard 3 texture.");
            Create<string>("texture_input_keyboard_key_4", "textures/input/Keyboard/Keyboard_Black_4", CVarFlags.PRESERVE, "Path of keyboard 4 texture.");
            Create<string>("texture_input_keyboard_key_5", "textures/input/Keyboard/Keyboard_Black_5", CVarFlags.PRESERVE, "Path of keyboard 5 texture.");
            Create<string>("texture_input_keyboard_key_6", "textures/input/Keyboard/Keyboard_Black_6", CVarFlags.PRESERVE, "Path of keyboard 6 texture.");
            Create<string>("texture_input_keyboard_key_7", "textures/input/Keyboard/Keyboard_Black_7", CVarFlags.PRESERVE, "Path of keyboard 7 texture.");
            Create<string>("texture_input_keyboard_key_8", "textures/input/Keyboard/Keyboard_Black_8", CVarFlags.PRESERVE, "Path of keyboard 8 texture.");
            Create<string>("texture_input_keyboard_key_9", "textures/input/Keyboard/Keyboard_Black_9", CVarFlags.PRESERVE, "Path of keyboard 9 texture.");

            Create<string>("texture_input_keyboard_key_a", "textures/input/Keyboard/Keyboard_Black_A", CVarFlags.PRESERVE, "Path of keyboard A texture.");
            Create<string>("texture_input_keyboard_key_b", "textures/input/Keyboard/Keyboard_Black_B", CVarFlags.PRESERVE, "Path of keyboard B texture.");
            Create<string>("texture_input_keyboard_key_c", "textures/input/Keyboard/Keyboard_Black_C", CVarFlags.PRESERVE, "Path of keyboard C texture.");
            Create<string>("texture_input_keyboard_key_d", "textures/input/Keyboard/Keyboard_Black_D", CVarFlags.PRESERVE, "Path of keyboard D texture.");
            Create<string>("texture_input_keyboard_key_e", "textures/input/Keyboard/Keyboard_Black_E", CVarFlags.PRESERVE, "Path of keyboard E texture.");
            Create<string>("texture_input_keyboard_key_f", "textures/input/Keyboard/Keyboard_Black_F", CVarFlags.PRESERVE, "Path of keyboard F texture.");
            Create<string>("texture_input_keyboard_key_g", "textures/input/Keyboard/Keyboard_Black_G", CVarFlags.PRESERVE, "Path of keyboard G texture.");
            Create<string>("texture_input_keyboard_key_h", "textures/input/Keyboard/Keyboard_Black_H", CVarFlags.PRESERVE, "Path of keyboard H texture.");
            Create<string>("texture_input_keyboard_key_i", "textures/input/Keyboard/Keyboard_Black_I", CVarFlags.PRESERVE, "Path of keyboard I texture.");
            Create<string>("texture_input_keyboard_key_j", "textures/input/Keyboard/Keyboard_Black_J", CVarFlags.PRESERVE, "Path of keyboard J texture.");
            Create<string>("texture_input_keyboard_key_k", "textures/input/Keyboard/Keyboard_Black_K", CVarFlags.PRESERVE, "Path of keyboard K texture.");
            Create<string>("texture_input_keyboard_key_l", "textures/input/Keyboard/Keyboard_Black_L", CVarFlags.PRESERVE, "Path of keyboard L texture.");
            Create<string>("texture_input_keyboard_key_m", "textures/input/Keyboard/Keyboard_Black_M", CVarFlags.PRESERVE, "Path of keyboard M texture.");
            Create<string>("texture_input_keyboard_key_n", "textures/input/Keyboard/Keyboard_Black_N", CVarFlags.PRESERVE, "Path of keyboard N texture.");
            Create<string>("texture_input_keyboard_key_o", "textures/input/Keyboard/Keyboard_Black_O", CVarFlags.PRESERVE, "Path of keyboard O texture.");
            Create<string>("texture_input_keyboard_key_p", "textures/input/Keyboard/Keyboard_Black_P", CVarFlags.PRESERVE, "Path of keyboard P texture.");
            Create<string>("texture_input_keyboard_key_q", "textures/input/Keyboard/Keyboard_Black_Q", CVarFlags.PRESERVE, "Path of keyboard Q texture.");
            Create<string>("texture_input_keyboard_key_r", "textures/input/Keyboard/Keyboard_Black_R", CVarFlags.PRESERVE, "Path of keyboard R texture.");
            Create<string>("texture_input_keyboard_key_s", "textures/input/Keyboard/Keyboard_Black_S", CVarFlags.PRESERVE, "Path of keyboard S texture.");
            Create<string>("texture_input_keyboard_key_t", "textures/input/Keyboard/Keyboard_Black_T", CVarFlags.PRESERVE, "Path of keyboard T texture.");
            Create<string>("texture_input_keyboard_key_u", "textures/input/Keyboard/Keyboard_Black_U", CVarFlags.PRESERVE, "Path of keyboard U texture.");
            Create<string>("texture_input_keyboard_key_v", "textures/input/Keyboard/Keyboard_Black_V", CVarFlags.PRESERVE, "Path of keyboard V texture.");
            Create<string>("texture_input_keyboard_key_w", "textures/input/Keyboard/Keyboard_Black_W", CVarFlags.PRESERVE, "Path of keyboard W texture.");
            Create<string>("texture_input_keyboard_key_x", "textures/input/Keyboard/Keyboard_Black_X", CVarFlags.PRESERVE, "Path of keyboard X texture.");
            Create<string>("texture_input_keyboard_key_y", "textures/input/Keyboard/Keyboard_Black_Y", CVarFlags.PRESERVE, "Path of keyboard Y texture.");
            Create<string>("texture_input_keyboard_key_z", "textures/input/Keyboard/Keyboard_Black_Z", CVarFlags.PRESERVE, "Path of keyboard Z texture.");

            Create<string>("texture_input_keyboard_key_up", "textures/input/Keyboard/Keyboard_Black_Arrow_Up", CVarFlags.PRESERVE, "Path of keyboard arrow up texture.");
            Create<string>("texture_input_keyboard_key_down", "textures/input/Keyboard/Keyboard_Black_Arrow_Down", CVarFlags.PRESERVE, "Path of keyboard arrow down texture.");
            Create<string>("texture_input_keyboard_key_left", "textures/input/Keyboard/Keyboard_Black_Arrow_Left", CVarFlags.PRESERVE, "Path of keyboard arrow left texture.");
            Create<string>("texture_input_keyboard_key_right", "textures/input/Keyboard/Keyboard_Black_Arrow_Right", CVarFlags.PRESERVE, "Path of keyboard arrow right texture.");

            Create<string>("texture_input_keyboard_key_bracket_left", "textures/input/Keyboard/Keyboard_Black_Bracket_Left", CVarFlags.PRESERVE, "Path of keyboard left bracket texture.");
            Create<string>("texture_input_keyboard_key_bracket_right", "textures/input/Keyboard/Keyboard_Black_Bracket_Right", CVarFlags.PRESERVE, "Path of keyboard right bracket texture.");
            Create<string>("texture_input_keyboard_key_semicolon", "textures/input/Keyboard/Keyboard_Black_Semicolon", CVarFlags.PRESERVE, "Path of keyboard semicolon texture.");
            Create<string>("texture_input_keyboard_key_quote", "textures/input/Keyboard/Keyboard_Black_Quote", CVarFlags.PRESERVE, "Path of keyboard quote texture.");
            Create<string>("texture_input_keyboard_key_mark_left", "textures/input/Keyboard/Keyboard_Black_Mark_Left", CVarFlags.PRESERVE, "Path of keyboard less than texture.");
            Create<string>("texture_input_keyboard_key_mark_right", "textures/input/Keyboard/Keyboard_Black_Mark_Right", CVarFlags.PRESERVE, "Path of keyboard greater than texture.");
            Create<string>("texture_input_keyboard_key_question", "textures/input/Keyboard/Keyboard_Black_Question", CVarFlags.PRESERVE, "Path of keyboard question mark texture.");
            Create<string>("texture_input_keyboard_key_slash", "textures/input/Keyboard/Keyboard_Black_Slash", CVarFlags.PRESERVE, "Path of keyboard slash texture.");
            Create<string>("texture_input_keyboard_key_minus", "textures/input/Keyboard/Keyboard_Black_Minus", CVarFlags.PRESERVE, "Path of keyboard minus texture.");
            Create<string>("texture_input_keyboard_key_plus", "textures/input/Keyboard/Keyboard_Black_Plus", CVarFlags.PRESERVE, "Path of keyboard plus texture.");
            Create<string>("texture_input_keyboard_key_space", "textures/input/Keyboard/Keyboard_Black_Space", CVarFlags.PRESERVE, "Path of keyboard space bar texture.");

            Create<string>("texture_input_keyboard_key_blank", "textures/input/Keyboard/Blank_Black_Normal", CVarFlags.PRESERVE, "Path of keyboard blank texture.");
            /** END KEYBOARD INPUT TEXTURES **/

            /** BEGIN GAMEPAD INPUT TEXTURES **/
            Create<string>("texture_input_gamepad_a", "textures/input/XboxOne/XboxOne_A", CVarFlags.PRESERVE, "Path of gamepad A button texture.");
            Create<string>("texture_input_gamepad_b", "textures/input/XboxOne/XboxOne_B", CVarFlags.PRESERVE, "Path of gamepad B button texture.");
            Create<string>("texture_input_gamepad_x", "textures/input/XboxOne/XboxOne_X", CVarFlags.PRESERVE, "Path of gamepad X button texture.");
            Create<string>("texture_input_gamepad_y", "textures/input/XboxOne/XboxOne_Y", CVarFlags.PRESERVE, "Path of gamepad Y button texture.");

            Create<string>("texture_input_gamepad_dpad_up", "textures/input/XboxOne/XboxOne_Dpad_Up", CVarFlags.PRESERVE, "Path of gamepad dpad up button texture.");
            Create<string>("texture_input_gamepad_dpad_down", "textures/input/XboxOne/XboxOne_Dpad_Down", CVarFlags.PRESERVE, "Path of gamepad dpad down button texture.");
            Create<string>("texture_input_gamepad_dpad_left", "textures/input/XboxOne/XboxOne_Dpad_Left", CVarFlags.PRESERVE, "Path of gamepad dpad left button texture.");
            Create<string>("texture_input_gamepad_dpad_right", "textures/input/XboxOne/XboxOne_Dpad_Right", CVarFlags.PRESERVE, "Path of gamepad dpad right button texture.");

            Create<string>("texture_input_gamepad_bumper_left", "textures/input/XboxOne/XboxOne_LB", CVarFlags.PRESERVE, "Path of gamepad left bumper button texture.");
            Create<string>("texture_input_gamepad_bumper_right", "textures/input/XboxOne/XboxOne_RB", CVarFlags.PRESERVE, "Path of gamepad right bumper button texture.");
            Create<string>("texture_input_gamepad_trigger_left", "textures/input/XboxOne/XboxOne_LT", CVarFlags.PRESERVE, "Path of gamepad left trigger button texture.");
            Create<string>("texture_input_gamepad_trigger_right", "textures/input/XboxOne/XboxOne_RT", CVarFlags.PRESERVE, "Path of gamepad right trigger button texture.");

            Create<string>("texture_input_gamepad_stick_left", "textures/input/XboxOne/XboxOne_Left_Stick", CVarFlags.PRESERVE, "Path of gamepad left stick button texture.");
            Create<string>("texture_input_gamepad_stick_right", "textures/input/XboxOne/XboxOne_Right_Stick", CVarFlags.PRESERVE, "Path of gamepad right stick button texture.");
            /** END GAMEPAD INPUT TEXTURES **/

            Create<float>("sound_master_volume", 1.0f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Master volume setting.");
            Create<float>("sound_music_volume", 1.0f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Music volume setting.");
            Create<float>("sound_effect_volume", 1.0f, CVarFlags.PRESERVE | CVarFlags.LIVE_RELOAD, "Sound effect volume setting.");

            Create<string>("sound_explosion", "sounds/Explosion_Sound", CVarFlags.PRESERVE, "Path of explosion sound.");
            Create<string>("sound_projectile_fired", "sounds/Laser_Shot", CVarFlags.PRESERVE, "Path of projectile fired sound.");
            Create<string>("sound_projectile_bounce", "sounds/Proj_Bounce", CVarFlags.PRESERVE, "Path of projectile bounce sound.");

            Create<string>("effect_blur", "effects/Blur", CVarFlags.PRESERVE, "Path of blur post-processing shader.");
            Create<string>("effect_fxaa", "effects/FXAA", CVarFlags.PRESERVE, "Path of FXAA post-processing shader.");
            Create<string>("effect_smaa", "effects/SMAA_ULTRA", CVarFlags.PRESERVE, "Path of SMAA post-processing shader.");
            Create<string>("effect_negative", "effects/Negative", CVarFlags.PRESERVE, "Path of negative post-processing shader.");

            Create<string>("effect_field_font", "effects/FieldFontEffect", CVarFlags.PRESERVE, "Effect of field font shader.");
            Create<string>("font_msdf_hyperspace", "fonts/Hyperspace/Hyperspace", CVarFlags.PRESERVE, "Path of hyperspace MSDF font descriptor file.");
            Create<string>("font_msdf_hyperspace_texture_character_32", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-32", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 32.");
            Create<string>("font_msdf_hyperspace_texture_character_33", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-33", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 33.");
            Create<string>("font_msdf_hyperspace_texture_character_34", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-34", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 34.");
            Create<string>("font_msdf_hyperspace_texture_character_35", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-35", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 35.");
            Create<string>("font_msdf_hyperspace_texture_character_36", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-36", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 36.");
            Create<string>("font_msdf_hyperspace_texture_character_37", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-37", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 37.");
            Create<string>("font_msdf_hyperspace_texture_character_38", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-38", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 38.");
            Create<string>("font_msdf_hyperspace_texture_character_39", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-39", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 39.");
            Create<string>("font_msdf_hyperspace_texture_character_40", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-40", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 40.");
            Create<string>("font_msdf_hyperspace_texture_character_41", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-41", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 41.");
            Create<string>("font_msdf_hyperspace_texture_character_42", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-42", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 42.");
            Create<string>("font_msdf_hyperspace_texture_character_43", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-43", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 43.");
            Create<string>("font_msdf_hyperspace_texture_character_44", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-44", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 44.");
            Create<string>("font_msdf_hyperspace_texture_character_45", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-45", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 45.");
            Create<string>("font_msdf_hyperspace_texture_character_46", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-46", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 46.");
            Create<string>("font_msdf_hyperspace_texture_character_47", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-47", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 47.");
            Create<string>("font_msdf_hyperspace_texture_character_48", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-48", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 48.");
            Create<string>("font_msdf_hyperspace_texture_character_49", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-49", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 49.");
            Create<string>("font_msdf_hyperspace_texture_character_50", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-50", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 50.");
            Create<string>("font_msdf_hyperspace_texture_character_51", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-51", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 51.");
            Create<string>("font_msdf_hyperspace_texture_character_52", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-52", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 52.");
            Create<string>("font_msdf_hyperspace_texture_character_53", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-53", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 53.");
            Create<string>("font_msdf_hyperspace_texture_character_54", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-54", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 54.");
            Create<string>("font_msdf_hyperspace_texture_character_55", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-55", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 55.");
            Create<string>("font_msdf_hyperspace_texture_character_56", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-56", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 56.");
            Create<string>("font_msdf_hyperspace_texture_character_57", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-57", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 57.");
            Create<string>("font_msdf_hyperspace_texture_character_58", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-58", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 58.");
            Create<string>("font_msdf_hyperspace_texture_character_59", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-59", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 59.");
            Create<string>("font_msdf_hyperspace_texture_character_60", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-60", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 60.");
            Create<string>("font_msdf_hyperspace_texture_character_61", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-61", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 61.");
            Create<string>("font_msdf_hyperspace_texture_character_62", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-62", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 62.");
            Create<string>("font_msdf_hyperspace_texture_character_63", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-63", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 63.");
            Create<string>("font_msdf_hyperspace_texture_character_64", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-64", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 64.");
            Create<string>("font_msdf_hyperspace_texture_character_65", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-65", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 65.");
            Create<string>("font_msdf_hyperspace_texture_character_66", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-66", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 66.");
            Create<string>("font_msdf_hyperspace_texture_character_67", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-67", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 67.");
            Create<string>("font_msdf_hyperspace_texture_character_68", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-68", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 68.");
            Create<string>("font_msdf_hyperspace_texture_character_69", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-69", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 69.");
            Create<string>("font_msdf_hyperspace_texture_character_70", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-70", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 70.");
            Create<string>("font_msdf_hyperspace_texture_character_71", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-71", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 71.");
            Create<string>("font_msdf_hyperspace_texture_character_72", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-72", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 72.");
            Create<string>("font_msdf_hyperspace_texture_character_73", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-73", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 73.");
            Create<string>("font_msdf_hyperspace_texture_character_74", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-74", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 74.");
            Create<string>("font_msdf_hyperspace_texture_character_75", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-75", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 75.");
            Create<string>("font_msdf_hyperspace_texture_character_76", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-76", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 76.");
            Create<string>("font_msdf_hyperspace_texture_character_77", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-77", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 77.");
            Create<string>("font_msdf_hyperspace_texture_character_78", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-78", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 78.");
            Create<string>("font_msdf_hyperspace_texture_character_79", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-79", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 79.");
            Create<string>("font_msdf_hyperspace_texture_character_80", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-80", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 80.");
            Create<string>("font_msdf_hyperspace_texture_character_81", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-81", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 81.");
            Create<string>("font_msdf_hyperspace_texture_character_82", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-82", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 82.");
            Create<string>("font_msdf_hyperspace_texture_character_83", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-83", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 83.");
            Create<string>("font_msdf_hyperspace_texture_character_84", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-84", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 84.");
            Create<string>("font_msdf_hyperspace_texture_character_85", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-85", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 85.");
            Create<string>("font_msdf_hyperspace_texture_character_86", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-86", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 86.");
            Create<string>("font_msdf_hyperspace_texture_character_87", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-87", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 87.");
            Create<string>("font_msdf_hyperspace_texture_character_88", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-88", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 88.");
            Create<string>("font_msdf_hyperspace_texture_character_89", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-89", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 89.");
            Create<string>("font_msdf_hyperspace_texture_character_90", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-90", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 90.");
            Create<string>("font_msdf_hyperspace_texture_character_91", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-91", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 91.");
            Create<string>("font_msdf_hyperspace_texture_character_92", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-92", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 92.");
            Create<string>("font_msdf_hyperspace_texture_character_93", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-93", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 93.");
            Create<string>("font_msdf_hyperspace_texture_character_94", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-94", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 94.");
            Create<string>("font_msdf_hyperspace_texture_character_95", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-95", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 95.");
            Create<string>("font_msdf_hyperspace_texture_character_96", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-96", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 96.");
            Create<string>("font_msdf_hyperspace_texture_character_97", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-97", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 97.");
            Create<string>("font_msdf_hyperspace_texture_character_98", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-98", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 98.");
            Create<string>("font_msdf_hyperspace_texture_character_99", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-99", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 99.");
            Create<string>("font_msdf_hyperspace_texture_character_100", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-100", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 100.");
            Create<string>("font_msdf_hyperspace_texture_character_101", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-101", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 101.");
            Create<string>("font_msdf_hyperspace_texture_character_102", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-102", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 102.");
            Create<string>("font_msdf_hyperspace_texture_character_103", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-103", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 103.");
            Create<string>("font_msdf_hyperspace_texture_character_104", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-104", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 104.");
            Create<string>("font_msdf_hyperspace_texture_character_105", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-105", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 105.");
            Create<string>("font_msdf_hyperspace_texture_character_106", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-106", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 106.");
            Create<string>("font_msdf_hyperspace_texture_character_107", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-107", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 107.");
            Create<string>("font_msdf_hyperspace_texture_character_108", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-108", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 108.");
            Create<string>("font_msdf_hyperspace_texture_character_109", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-109", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 109.");
            Create<string>("font_msdf_hyperspace_texture_character_110", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-110", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 110.");
            Create<string>("font_msdf_hyperspace_texture_character_111", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-111", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 111.");
            Create<string>("font_msdf_hyperspace_texture_character_112", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-112", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 112.");
            Create<string>("font_msdf_hyperspace_texture_character_113", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-113", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 113.");
            Create<string>("font_msdf_hyperspace_texture_character_114", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-114", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 114.");
            Create<string>("font_msdf_hyperspace_texture_character_115", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-115", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 115.");
            Create<string>("font_msdf_hyperspace_texture_character_116", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-116", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 116.");
            Create<string>("font_msdf_hyperspace_texture_character_117", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-117", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 117.");
            Create<string>("font_msdf_hyperspace_texture_character_118", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-118", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 118.");
            Create<string>("font_msdf_hyperspace_texture_character_119", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-119", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 119.");
            Create<string>("font_msdf_hyperspace_texture_character_120", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-120", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 120.");
            Create<string>("font_msdf_hyperspace_texture_character_121", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-121", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 121.");
            Create<string>("font_msdf_hyperspace_texture_character_122", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-122", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 122.");
            Create<string>("font_msdf_hyperspace_texture_character_123", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-123", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 123.");
            Create<string>("font_msdf_hyperspace_texture_character_124", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-124", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 124.");
            Create<string>("font_msdf_hyperspace_texture_character_125", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-125", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 125.");
            Create<string>("font_msdf_hyperspace_texture_character_126", "fonts/Hyperspace/Hyperspace_MSDF/Hyperspace-126", CVarFlags.PRESERVE, "Path of hyperspace MSDF character texture 126.");

            Create<string>("ui_main_menu", "ui/MainMenu", CVarFlags.PRESERVE, "Path of UI file for UIMenuGameState.");
            Create<string>("ui_options_menu", "ui/OptionsMenu", CVarFlags.PRESERVE, "Path of UI file for UIOptionsGameState.");
            Create<string>("ui_pause_menu", "ui/PauseMenu", CVarFlags.PRESERVE, "Path of UI file for pause menu.");
            Create<string>("ui_adrift_game_ui", "ui/MainGameStateUI", CVarFlags.PRESERVE, "Path of UI file for adrift gameplay UI.");

            Create<string>("ui_lobby_menu", "ui/Lobby/LobbyMenu", CVarFlags.PRESERVE, "Path of UI file for the lobby menu.");
            Create<string>("ui_lobby_menu_join_instructions", "ui/Lobby/JoinInstructions", CVarFlags.PRESERVE, "Path of UI file for join instructions external UI for the lobby menu.");

            Create<string>("ui_game_over", "ui/GameOverScoreUI", CVarFlags.PRESERVE, "Path of UI file for game over/score screen.");

            Create<string>("ui_test", "ui/test", CVarFlags.PRESERVE, "Path of test UI 1.");
            Create<string>("ui_test2", "ui/test2", CVarFlags.PRESERVE, "Path of test UI 2.");
        }
    }
}
