namespace GameJam.Constants
{
    public class GamePlay
    {
        public static readonly float PLAYER_SHIELD_RADIUS = 30.0f;
        public static readonly  int  PLAYER_SHIP_MAX_HEALTH = 3;

        public static readonly float KAMIKAZE_ENEMY_SPEED = 30.0f;
        public static readonly float KAMIKAZE_ROTATION_SPEED = 0.5f;
        public static readonly float KAMIKAZE_PUSHBACK_FORCE = 5;

        public static readonly float SHOOTING_ENEMY_SPEED = 0.0f;
        public static readonly float SHOOTING_ENEMY_ROTATION_SPEED = 1.5f;
        public static readonly float SHOOTING_ENEMY_PROJECETILE_SPEED = 40.0f;
        public static readonly  int  SHOOTING_ENEMY_PROJECTILE_BOUNCES = 3;
        public static readonly  int  SHOOTING_ENEMY_PROJECTILE_AMMO = 4;

        public static readonly float SPAWNER_KAMIKAZE_INITIAL_PERIOD = 3;
        public static readonly float SPAWNER_KAMIKAZE_PERIOD_MULTIPLIER = 0.999f;

        public static readonly float SPAWNER_SHOOTING_ENEMY_INITIAL_PERIOD = 10;
        public static readonly float SPAWNER_SHOOTING_ENEMY_PERIOD_MULTIPLIER = 0.999f;

        public static readonly float SPANWER_MIN_DISTANCE_AWAY_FROM_PLAYER = 200.0f;
    }
}
