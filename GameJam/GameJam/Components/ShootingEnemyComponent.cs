using Audrey;

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
