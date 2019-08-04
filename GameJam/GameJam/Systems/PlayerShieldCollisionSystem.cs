using System;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class PlayerShieldCollisionSystem : BaseSystem
    {
        readonly Family playerShieldFamily = Family.All(typeof(PlayerShieldComponent), typeof(TransformComponent)).Get();
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();
        readonly Family collisionFamily = Family.All(typeof(CollisionComponent), typeof(TransformComponent)).Get();

        readonly ImmutableList<Entity> playerShieldEntities;
        readonly ImmutableList<Entity> collisionEntities;

        public PlayerShieldCollisionSystem(Engine engine) : base(engine)
        {
            playerShieldEntities = Engine.GetEntitiesFor(playerShieldFamily);
            collisionEntities = Engine.GetEntitiesFor(collisionFamily);
        }

        public override void Update(float dt)
        {
            foreach(Entity playerShieldEntity in playerShieldEntities)
            {
                TransformComponent transformComp = playerShieldEntity.GetComponent<TransformComponent>();
                PlayerShieldComponent shieldComp = playerShieldEntity.GetComponent<PlayerShieldComponent>();

                if (shieldComp.ShipEntity == null)
                {
                    return;
                }
                if (!playerShipFamily.Matches(shieldComp.ShipEntity))
                {
                    throw new Exception("Player shield does not have a valid ship entity within PlayerShieldComponent.");
                }
                Entity playerShip = shieldComp.ShipEntity;
                TransformComponent shipTransformComp = playerShip.GetComponent<TransformComponent>();

                Vector2 shieldNormal = transformComp.Position - shipTransformComp.Position;
                shieldNormal.Normalize();
                Vector2 shieldTangent = new Vector2(-shieldNormal.Y, shieldNormal.X);

                foreach (Entity collisionEntity in collisionEntities)
                {
                    TransformComponent collisionTransformComp = collisionEntity.GetComponent<TransformComponent>();
                    CollisionComponent collisionComp = collisionEntity.GetComponent<CollisionComponent>();

                    Vector2[] collisionEntityCorners = {
                        new Vector2(-collisionComp.BoundingBoxComponent.Width / 2, -collisionComp.BoundingBoxComponent.Height / 2),
                        new Vector2(-collisionComp.BoundingBoxComponent.Width / 2, collisionComp.BoundingBoxComponent.Height / 2),
                        new Vector2(collisionComp.BoundingBoxComponent.Width / 2, collisionComp.BoundingBoxComponent.Height / 2),
                        new Vector2(collisionComp.BoundingBoxComponent.Width / 2, -collisionComp.BoundingBoxComponent.Height / 2)
                    };
                    foreach (Vector2 collisionCorner in collisionEntityCorners)
                    {
                        Vector2 shieldCenterToCorner = transformComp.Position - (collisionCorner + collisionTransformComp.Position);
                        // Cast center-to-corner onto normal and tangent
                        float normalCasted = Vector2.Dot(shieldCenterToCorner, shieldNormal);
                        float tangentCasted = Vector2.Dot(shieldCenterToCorner, shieldTangent);

                        if(Math.Abs(normalCasted) <= shieldComp.Bounds.Y && Math.Abs(tangentCasted) <= shieldComp.Bounds.X)
                        {
                            if(!collisionComp.collidingWith.Contains(playerShieldEntity))
                            {
                                collisionComp.collidingWith.Add(playerShieldEntity);
                                EventManager.Instance.QueueEvent(new CollisionStartEvent(playerShieldEntity, collisionEntity));
                            }
                        } else
                        {
                            if (collisionComp.collidingWith.Contains(playerShieldEntity))
                            {
                                collisionComp.collidingWith.Remove(playerShieldEntity);
                                EventManager.Instance.QueueEvent(new CollisionEndEvent(playerShieldEntity, collisionEntity));
                            }
                        }
                    }
                }
            }
        }
    }
}
