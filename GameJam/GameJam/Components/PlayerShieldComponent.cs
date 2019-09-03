using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class PlayerShieldComponent : IComponent
    {
        public PlayerShieldComponent(Entity shipEntity, float angle = 0)
            : this(shipEntity, angle, CVars.Get<float>("player_shield_radius"),
                  Constants.ObjectBounds.PLAYER_SHIELD_BOUNDS)
        {
        }
        public PlayerShieldComponent(Entity shipEntity, float angle, float radius,
            Vector2 bounds)
        {
            ShipEntity = shipEntity;
            Radius = radius;
            Angle = angle;
            Bounds = bounds;
        }

        public Entity ShipEntity;
        public float Radius;
        public float Angle;
        public Vector2 Bounds;
    }
}
