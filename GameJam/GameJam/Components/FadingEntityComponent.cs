using Audrey;
using System;

namespace GameJam.Components
{
    /*
     * Add this component to an entity if you plan to draw it repetively using the FadingEntitySystem.
     * Pass in the type of the entity so that generic "DrawEntityOnly" can be made.
     */
    public class FadingEntityComponent : IComponent
    {
        public Type thisType;

        public FadingEntityComponent(Type type)
        {
            thisType = type;
        }
    }
}
