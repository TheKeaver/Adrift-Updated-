using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ShootingEnemyComponent : IComponent
    {
        public int ammoLeft;

        public ShootingEnemyComponent(int totalAmmo)
        {
            ammoLeft = totalAmmo;
        }
    }
}
