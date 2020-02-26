namespace GameJam.Input
{
    /// <summary>
    /// A snapshot of an input method.
    /// </summary>
    public class InputSnapshot
    {
        public float Angle
        {
            get;
            internal set;
        }

        public bool SuperShield
        {
            get;
            internal set;
        }

        public InputSnapshot() : this(0)
        {
        }

        public InputSnapshot(float angle)
        {
            Angle = angle;
        }
    }
}
