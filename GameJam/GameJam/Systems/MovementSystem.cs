using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class MovementSystem : BaseSystem
    {
        Family _movementFamily = Family.All(typeof(MovementComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _movementEntities;

        public MovementSystem(Engine engine) : base(engine)
        {
            _movementEntities = engine.GetEntitiesFor(_movementFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity movementEntity in _movementEntities)
            {
                processMovement(movementEntity, dt);
            }
        }

        void processMovement(Entity movementEntity, float dt)
        {
            TransformComponent transformComp = movementEntity.GetComponent<TransformComponent>();
            MovementComponent movementComp = movementEntity.GetComponent<MovementComponent>();

            transformComp.Move(movementComp.speed * movementComp.direction * dt);
            float targetAngle = (float)Math.Atan2(movementComp.direction.Y, movementComp.direction.X);
            transformComp.Rotate(targetAngle - transformComp.Rotation);
        }
    }
}
