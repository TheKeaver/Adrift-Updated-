using Audrey;
using System;

namespace GameJam.Components
{
    public class LaserBeamReflectionComponent : IComponent, ICopyComponent
    {
        public Player ReflectedBy = null;

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            //TODO: Fix this whatnot
            LaserBeamReflectionComponent lbrc = new LaserBeamReflectionComponent();
            lbrc.ReflectedBy = ReflectedBy;
            return lbrc;
        }
    }
}
