using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class EnemyRotationSystem : BaseSystem
    {
        Family _rotationFamily = Family.All(typeof(RotationComponent), typeof(EnemyComponent), typeof(MovementComponent), typeof(TransformComponent), typeof(CollisionComponent)).Get();
        ImmutableList<Entity> _rotationEntities;

        Family _playerFamily = Family.All(typeof(TransformComponent), typeof(PlayerShipComponent)).Get();
        ImmutableList<Entity> _playerEntities;

        public EnemyRotationSystem(Engine engine) : base(engine)
        {
            _rotationEntities = engine.GetEntitiesFor(_rotationFamily);
            _playerEntities = engine.GetEntitiesFor(_playerFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity rotationEntity in _rotationEntities)
            {
                TransformComponent rotationTransform = rotationEntity.GetComponent<TransformComponent>();
                float closestDistance = 100000;
                Entity closestPlayer = null;

                foreach (Entity playerEntity in _playerEntities)
                {
                    TransformComponent playerTransform = playerEntity.GetComponent<TransformComponent>();
                    float distance = (rotationTransform.Position - playerTransform.Position).Length();

                    if (closestDistance > distance)
                    {
                        closestDistance = distance;
                        closestPlayer = playerEntity;
                    }
                }

                if (closestPlayer != null)
                {
                    Vector2 current = rotationEntity.GetComponent<MovementComponent>().direction;
                    Vector2 target = closestPlayer.GetComponent<TransformComponent>().Position - rotationTransform.Position;

                    float currentAngle = (float)Math.Atan2(current.Y, current.X);
                    float targetAngle = (float)Math.Atan2(target.Y, target.X);

                    currentAngle = MathHelper.WrapAngle(currentAngle);
                    targetAngle = MathHelper.WrapAngle(targetAngle);

                    float diff = targetAngle - currentAngle;
                    float newAngle = currentAngle;

                    if (diff > 0)
                    {
                        if (Math.Abs(diff) < Math.PI)
                        {
                            newAngle += rotationEntity.GetComponent<RotationComponent>().rotationSpeed * dt;
                        }
                        else
                        {
                            newAngle -= rotationEntity.GetComponent<RotationComponent>().rotationSpeed * dt;
                        }
                    }
                    else
                    {
                        if(Math.Abs(diff) < Math.PI)
                        {
                            newAngle -= rotationEntity.GetComponent<RotationComponent>().rotationSpeed * dt;
                        }
                        else
                        {
                            newAngle += rotationEntity.GetComponent<RotationComponent>().rotationSpeed * dt;
                        }
                    }

                    Vector2 newDirection = new Vector2( (float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                    rotationEntity.GetComponent<MovementComponent>().direction = newDirection;
                }
            }
        }
    }
}
