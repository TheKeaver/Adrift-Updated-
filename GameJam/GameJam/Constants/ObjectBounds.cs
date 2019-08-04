using Microsoft.Xna.Framework;

namespace GameJam.Constants
{
    public class ObjectBounds
    {
        public static Vector2 PLAYER_SHIP_BOUNDS
        {
            get
            {
                return new Vector2(32, 32);
            }
        }
        public static Vector2 PLAYER_SHIELD_BOUNDS
        {
            get
            {
                return new Vector2(32, 10);
            }
        }
        public static Vector2 KAMIKAZE_SHIP_BOUNDS
        {
            get
            {
                return new Vector2(25, 25);
            }
        }
        public static Vector2 EXPLOSION_BOUNDS
        {
            get
            {
                return new Vector2(60, 60);
            }
        }
    }
}
