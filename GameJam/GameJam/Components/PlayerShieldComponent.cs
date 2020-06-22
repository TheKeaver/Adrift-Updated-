using Audrey;

namespace GameJam.Components
{
    public class PlayerShieldComponent : IComponent
    {
        public PlayerShieldComponent(Entity shipEntity, float angle = 0)
            : this(shipEntity, angle, CVars.Get<float>("player_shield_radius"), true)
        {
        }
        public PlayerShieldComponent(Entity shipEntity, float angle, float radius, bool active)
        {
            ShipEntity = shipEntity;
            Radius = radius;
            Angle = angle;
            Offset = angle;
            LaserReflectionActive = active;
        }

        public Entity ShipEntity;
        public float Radius;
        public float Angle;
        public float Offset;
        public bool LaserReflectionActive;
    }
}
