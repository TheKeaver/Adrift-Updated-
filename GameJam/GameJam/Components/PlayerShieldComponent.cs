using Audrey;

namespace GameJam.Components
{
    public class PlayerShieldComponent : IComponent
    {
        public PlayerShieldComponent(Entity shipEntity, float angle = 0)
            : this(shipEntity, angle, Constants.GamePlay.PLAYER_SHIELD_RADIUS)
        {
        }
        public PlayerShieldComponent(Entity shipEntity, float angle, float radius)
        {
            ShipEntity = shipEntity;
            Radius = radius;
            Angle = angle;
        }

        public Entity ShipEntity;
        public float Radius;
        public float Angle;
    }
}
