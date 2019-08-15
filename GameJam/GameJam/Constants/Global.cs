namespace GameJam.Constants
{
    /// <summary>
    /// Global engine constants.
    /// </summary>
    public class Global
    {
        public static readonly int WINDOW_WIDTH = 960;
        public static readonly int WINDOW_HEIGHT = 600;
        public static readonly float WINDOW_ASPECT_RATIO = (float)WINDOW_WIDTH / WINDOW_HEIGHT;

        public static readonly int SCREEN_WIDTH = 1280;
        public static readonly int SCREEN_HEIGHT = 800;
        public static readonly float SCREEN_ASPECT_RATIO = (float)SCREEN_WIDTH / SCREEN_HEIGHT;

        public static readonly float TICK_RATE = 1 / 120f;
    }
}
