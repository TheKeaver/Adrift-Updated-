using Audrey;

namespace GameJam.Components
{
    public class PlayerShipComponent : IComponent
    {
        public int LifeRemaining;
        public Entity ShipShield;
        public bool IsCollidingWithWall;

        public PlayerShipComponent(int maxLife)
        {
            LifeRemaining = maxLife;
            IsCollidingWithWall = false;
        }
    }
}
