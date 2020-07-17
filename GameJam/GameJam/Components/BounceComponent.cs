using Audrey;
using System;

namespace GameJam.Components
{
    public class BounceComponent : IComponent, ICopyComponent
    {
        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new BounceComponent();
        }
    }
}
