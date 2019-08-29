using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ShootingEnemyComponent : IComponent
    {
        public int AmmoLeft;

        public ShootingEnemyComponent(int totalAmmo)
        {
            AmmoLeft = totalAmmo;
        }
    }
}
