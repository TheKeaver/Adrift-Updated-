using Audrey;

namespace GameJam.Components
{
    public class PlayerShipComponent : IComponent
    {
        public int LifeRemaining;
        public Entity ShipShield;

        public PlayerShipComponent(int maxLife)
        {
            LifeRemaining = maxLife;
        }
    }
}
