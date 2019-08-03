using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class KamikazeSystem : BaseSystem
    {
        Family _kamikazeFamily = Family.All(typeof(KamikazeComponent), typeof(EnemyComponent), typeof(MovementComponent), typeof(TransformComponent), typeof(CollisionComponent)).Get();
        ImmutableList<Entity> _kamikazeEntities;

        Family _playerFamily = Family.All(typeof(TransformComponent), typeof(PlayerShipComponent)).Get();
        ImmutableList<Entity> _playerEntities;

        public KamikazeSystem(Engine engine) : base(engine)
        {
            _kamikazeEntities = engine.GetEntitiesFor(_kamikazeFamily);
            _playerEntities = engine.GetEntitiesFor(_playerFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity kamikazeEntity in _kamikazeEntities)
            {
                TransformComponent kamikazeTransform = kamikazeEntity.GetComponent<TransformComponent>();
                float closestDistance = 100000;
                Entity closestPlayer = null;

                foreach (Entity playerEntity in _playerEntities)
                {
                    TransformComponent playerTransform = playerEntity.GetComponent<TransformComponent>();
                    float distance = (kamikazeTransform.Position - playerTransform.Position).Length();

                    if (closestDistance > distance)
                    {
                        closestDistance = distance;
                        closestPlayer = playerEntity;
                    }
                }

                if (closestPlayer != null)
                {
                    Vector2 current = kamikazeEntity.GetComponent<MovementComponent>().direction;
                    Vector2 target = closestPlayer.GetComponent<TransformComponent>().Position - kamikazeTransform.Position;

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
                            newAngle += Constants.GamePlay.KAMIKAZE_ROTATION_SPEED * dt;
                        } else
                        {
                            newAngle -= Constants.GamePlay.KAMIKAZE_ROTATION_SPEED * dt;
                        }
                    }
                    else
                    {
                        if(Math.Abs(diff) < Math.PI)
                        {
                            newAngle -= Constants.GamePlay.KAMIKAZE_ROTATION_SPEED * dt;
                        } else
                        {
                            newAngle += Constants.GamePlay.KAMIKAZE_ROTATION_SPEED * dt;
                        }
                    }

                    Vector2 newDirection = new Vector2( (float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
                    kamikazeEntity.GetComponent<MovementComponent>().direction = newDirection;
                }
            }
        }
    }
}
