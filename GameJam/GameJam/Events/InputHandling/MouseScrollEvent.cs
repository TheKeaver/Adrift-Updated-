using System;
using Events;

namespace GameJam.Events.InputHandling
{
    public class MouseScrollEvent : IEvent
    {
        public float Value;
        public float Delta;

        public MouseScrollEvent(float value, float delta)
        {
            Value = value;
            Delta = delta;
        }
    }
}
