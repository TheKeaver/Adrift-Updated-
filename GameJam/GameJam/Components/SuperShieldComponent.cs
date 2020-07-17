using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class SuperShieldComponent : IComponent, ICopyComponent
    {
        public Entity ship;

        public SuperShieldComponent(Entity ship)
        {
            this.ship = ship;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            Entity e = GetOrMakeCopy(ship);
            return new SuperShieldComponent(e);
        }
    }
}
