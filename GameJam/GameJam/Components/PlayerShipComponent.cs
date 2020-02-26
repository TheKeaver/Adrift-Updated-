using Audrey;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class PlayerShipComponent : IComponent
    {
        public int LifeRemaining;
        public List<Entity> ShipShields;
        public bool IsCollidingWithWall;
        public float SuperShieldMeter;

        public PlayerShipComponent(int maxLife)
        {
            ShipShields = new List<Entity>();
            LifeRemaining = maxLife;
            IsCollidingWithWall = false;
            SuperShieldMeter = CVars.Get<float>("max_super_shield");
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
