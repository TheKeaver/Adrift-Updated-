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
        readonly Family _rotationFamily = Family.All(typeof(RotationComponent), typeof(EnemyComponent), typeof(MovementComponent), typeof(TransformComponent), typeof(CollisionComponent)).Get();
        readonly ImmutableList<Entity> _rotationEntities;

        readonly Family _playerFamily = Family.All(typeof(TransformComponent), typeof(PlayerShipComponent)).Get();
        readonly ImmutableList<Entity> _playerEntities;

        public EnemyRotationSystem(Engine engine) : base(engine)
        {
            _rotationEntities = engine.GetEntitiesFor(_rotationFamily);
            _playerEntities = engine.GetEntitiesFor(_playerFamily);
        }

        protected override void OnUpdate(float dt)
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
                    float current = rotationEntity.GetComponent<TransformComponent>().Rotation;

                    Vector2 target = closestPlayer.GetComponent<TransformComponent>().Position - rotationTransform.Position;

                    float targetAngle = (float)Math.Atan2(target.Y, target.X);

                    current = MathHelper.WrapAngle(current);
                    targetAngle = MathHelper.WrapAngle(targetAngle);

                    float diff = targetAngle - current;
                    float newAngle = current;

                    if (diff > 0)
                    {
                        if (Math.Abs(diff) < Math.PI)
                        {
                            newAngle += rotationEntity.GetComponent<RotationComponent>().RotationSpeed * dt;
                        }
                        else
                        {
                            newAngle -= rotationEntity.GetComponent<RotationComponent>().RotationSpeed * dt;
                        }
                    }
                    else
                    {
                        if(Math.Abs(diff) < Math.PI)
                        {
                            newAngle -= rotationEntity.GetComponent<RotationComponent>().RotationSpeed * dt;
                        }
                        else
                        {
                            newAngle += rotationEntity.GetComponent<RotationComponent>().RotationSpeed * dt;
                        }
                    }

                    Vector2 newDirection = new Vector2( (float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                    rotationEntity.GetComponent<MovementComponent>().MovementVector = newDirection * rotationEntity.GetComponent<MovementComponent>().MovementVector.Length();
                    rotationEntity.GetComponent<TransformComponent>().Rotate(newAngle - current);
                }
            }
        }
    }
}
