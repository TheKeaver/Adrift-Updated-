using Audrey;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class PlayerShipComponent : IComponent
    {
        public int LifeRemaining;
        public List<Entity> ShipShields;
        public bool IsCollidingWithWall;
        public bool SuperShieldAvailable;
        public float SuperShieldMeter;

        public PlayerShipComponent(int maxLife, float maxMeter)
        {
            ShipShields = new List<Entity>();
            LifeRemaining = maxLife;
            IsCollidingWithWall = false;
            SuperShieldAvailable = true;
            SuperShieldMeter = maxMeter;
        }

        public bool AddShield(Entity shield)
        {
            if (shield.HasComponent<PlayerShieldComponent>())
            {
                ShipShields.Add(shield);
                return true;
            }
            else
                return false;
        }
    }
}
