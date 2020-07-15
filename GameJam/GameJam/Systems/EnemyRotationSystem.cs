using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;

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

        public void Update(float dt)
        {
            foreach (Entity rotationEntity in _rotationEntities)
            {
                TransformComponent rotationTransform = rotationEntity.GetComponent<TransformComponent>();
                float closestDistance = float.PositiveInfinity;
                Entity closestPlayer = null;

                foreach (Entity playerEntity in _playerEntities)
                {
                    TransformComponent playerTransform = playerEntity.GetComponent<TransformComponent>();
                    float distance = (rotationTransform.Position - playerTransform.Position).Length();

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = playerEntity;
                    }
                }

                if (closestPlayer != null)
                {
                    float currentRotation = rotationEntity.GetComponent<TransformComponent>().Rotation;

                    Vector2 target = closestPlayer.GetComponent<TransformComponent>().Position - rotationTransform.Position;

                    float targetAngle = (float)Math.Atan2(target.Y, target.X);

                    currentRotation = MathHelper.WrapAngle(currentRotation);
                    targetAngle = MathHelper.WrapAngle(targetAngle);

                    float diff = targetAngle - currentRotation;
                    float newAngle = currentRotation;

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
                    rotationEntity.GetComponent<TransformComponent>().Rotate(newAngle - currentRotation);
                }
            }
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnKill()
        {
            return;
        }

        protected override void OnTogglePause()
        {
            return;
        }

        protected override void OnUpdate(float dt)
        {
            Update(dt);
        }
    }
}
