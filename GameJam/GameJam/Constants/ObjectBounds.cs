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
    }
}
