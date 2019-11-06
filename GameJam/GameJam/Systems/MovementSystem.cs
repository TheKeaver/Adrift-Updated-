using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class MovementSystem : BaseSystem
    {
        readonly Family _movementFamily = Family.All(typeof(MovementComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _movementEntities;

        public MovementSystem(Engine engine) : base(engine)
        {
            _movementEntities = engine.GetEntitiesFor(_movementFamily);
        }

        protected override void OnUpdate(float dt)
        {
            foreach (Entity movementEntity in _movementEntities)
            {
                ProcessMovement(movementEntity, dt);
            }
        }

        void ProcessMovement(Entity movementEntity, float dt)
        {
            TransformComponent transformComp = movementEntity.GetComponent<TransformComponent>();
            MovementComponent movementComp = movementEntity.GetComponent<MovementComponent>();

            transformComp.Move(movementComp.MovementVector * dt);
            if (movementComp.UpdateRotationWithDirection && movementComp.MovementVector.Length() != 0)
            {
                float targetAngle = (float)Math.Atan2(movementComp.MovementVector.Y, movementComp.MovementVector.X);
                transformComp.Rotate(targetAngle - transformComp.Rotation);
            }
        }
    }
}
