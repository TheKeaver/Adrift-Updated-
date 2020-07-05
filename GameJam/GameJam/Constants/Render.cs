namespace GameJam.Constants
{
    /// <summary>
    /// Constants for rendering.
    /// </summary>
    public class Render
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
        public static readonly byte RENDER_GROUP_STARS = GROUP_ONE;
        public static readonly byte RENDER_GROUP_GAME_ENTITIES = GROUP_TWO;
        public static readonly byte RENDER_GROUP_NO_GLOW = GROUP_THREE;
        public static readonly byte RENDER_GROUP_UI = GROUP_FOUR;

        //////////////
        public static readonly float RENDER_DEPTH_LAYER_SPRITES = 0;
        public static readonly float RENDER_DEPTH_LAYER_SPRITES_BACKGROUND = RENDER_DEPTH_LAYER_SPRITES;
        public static readonly float RENDER_DEPTH_LAYER_SPRITES_GAMEPLAY = RENDER_DEPTH_LAYER_SPRITES_BACKGROUND + 0.1f;

        public static readonly float RENDER_DEPTH_LAYER_UI = RENDER_DEPTH_LAYER_SPRITES * 10000000000;
        public static readonly float RENDER_DEPTH_LAYER_UI_LAYER_DIVIDER = 100;
        public static float GetUIRenderDepth(int uiLayer, int layerIndex)
        {
            return RENDER_DEPTH_LAYER_UI + uiLayer * RENDER_DEPTH_LAYER_UI_LAYER_DIVIDER + layerIndex;
        }
    }
}
