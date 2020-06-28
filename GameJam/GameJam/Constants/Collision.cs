using System.Text.RegularExpressions;

namespace GameJam.Constants
{
    /// <summary>
    /// Constants for collision groups.
    /// </summary>
    public class Collision
    {
        public static readonly byte GROUP_ONE = 0x1;
        public static readonly byte GROUP_TWO = 0x2;
        public static readonly byte GROUP_THREE = 0x4;
        public static readonly byte GROUP_FOUR = 0x8;
        public static readonly byte GROUP_FIVE = 0x10;
        public static readonly byte GROUP_SIX = 0x20;
        public static readonly byte GROUP_SEVEN = 0x40;
        public static readonly byte GROUP_EIGHT = 0x80;

        public static readonly byte GROUP_MASK_NONE = 0x0;
        public static readonly byte GROUP_MASK_ALL = 0xFF;

        ///////////////
        public static readonly byte COLLISION_GROUP_PLAYER = GROUP_ONE;
        public static readonly byte COLLISION_GROUP_ENEMIES = GROUP_TWO;
        public static readonly byte COLLISION_GROUP_EDGES = GROUP_THREE;
        public static readonly byte COLLISION_GROUP_RAYCAST = GROUP_EIGHT;
    }
}
