using Events;

namespace GameJam.Events
{
    /// <summary>
    /// An event queued when the window is resized.
    /// </summary>
    public class ResizeEvent : IEvent
    {
        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }

        public ResizeEvent(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
