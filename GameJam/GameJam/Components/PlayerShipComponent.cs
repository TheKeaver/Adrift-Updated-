using Audrey;
using System;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class PlayerShipComponent : IComponent, ICopyComponent
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

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new PlayerShipComponent(LifeRemaining, SuperShieldMeter);
        }
    }
}
