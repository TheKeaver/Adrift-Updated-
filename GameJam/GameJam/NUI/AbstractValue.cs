namespace GameJam.NUI
{
    public interface AbstractValue
    {
        float Value
        {
            get;
        }
    }

    public class FixedValue : AbstractValue
    {
        public float Value
        {
            get;
            private set;
        }

        public FixedValue(float value)
        {
            Value = value;
        }
    }
}
