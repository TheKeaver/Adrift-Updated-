using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class SuperShieldComponent : IComponent
    {
        public Entity ship;

        public SuperShieldComponent(Entity ship)
        {
            this.ship = ship;
        }
    }
}
