using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class PlayerShieldSystem : BaseSystem
    {
        readonly Family playerShieldFamily = Family.All(typeof(PlayerShieldComponent), typeof(TransformComponent)).Get();
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();

        readonly ImmutableList<Entity> playerShieldEntities;

        public PlayerShieldSystem(Engine engine) : base(engine)
        {
            playerShieldEntities = engine.GetEntitiesFor(playerShieldFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity playerShield in playerShieldEntities)
            {
                UpdateAngleFromInput(playerShield);
                UpdateTransform(playerShield);
            }
        }

        private void UpdateAngleFromInput(Entity playerShield)
        {
            PlayerShieldComponent shieldComp = playerShield.GetComponent<PlayerShieldComponent>();
            PlayerComponent playerComp = playerShield.GetComponent<PlayerComponent>();
            if(playerComp != null)
            {
                shieldComp.Angle = playerComp.Player.InputMethod.GetSnapshot().Angle;
            }
        }
        private void UpdateTransform(Entity playerShield)
        {
            PlayerShieldComponent shieldComp = playerShield.GetComponent<PlayerShieldComponent>();

            if (shieldComp.ShipEntity == null)
            {
                return;
            }
            if (!playerShipFamily.Matches(shieldComp.ShipEntity))
            {
                throw new Exception("Player shield does not have a valid ship entity within PlayerShieldComponent.");
            }
            Entity playerShip = shieldComp.ShipEntity;

            TransformComponent shieldTransformComp = playerShield.GetComponent<TransformComponent>();
            TransformComponent shipTransformComp = playerShip.GetComponent<TransformComponent>();

            Vector2 shieldTargetPosition = shipTransformComp.Position
                + new Vector2((float)(shieldComp.Radius * Math.Cos(shieldComp.Angle)),
                    (float)(shieldComp.Radius * Math.Sin(shieldComp.Angle)));

            shieldTransformComp.Move(shieldTargetPosition - shieldTransformComp.Position);

            Vector2 shieldToShip = shieldTargetPosition - shipTransformComp.Position;
            float targetRotation = (float)Math.Atan2(shieldToShip.Y, shieldToShip.X) + (float)(Math.PI / 2); // PI/2 because sprite is rotated 90deg wrong
            shieldTransformComp.Rotate(targetRotation - shieldTransformComp.Rotation);
        }
    }
}
