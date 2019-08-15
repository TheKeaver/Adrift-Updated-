using Audrey;

namespace GameJam.Components
{
    public class PlayerShipComponent : IComponent
    {
        public int lifeRemaining;
        public Entity shipShield;

        public PlayerShipComponent(int maxLife)
        {
            lifeRemaining = maxLife;
        }
    }
}
