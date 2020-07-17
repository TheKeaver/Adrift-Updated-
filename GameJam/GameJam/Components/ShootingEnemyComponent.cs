using Audrey;
using System;

namespace GameJam.Components
{
    public class ShootingEnemyComponent : IComponent, ICopyComponent
    {
        public int AmmoLeft;

        public ShootingEnemyComponent(int totalAmmo)
        {
            AmmoLeft = totalAmmo;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new ShootingEnemyComponent(AmmoLeft);
        }
    }
}
