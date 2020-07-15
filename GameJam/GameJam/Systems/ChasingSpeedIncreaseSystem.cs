using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class ChasingSpeedIncreaseSystem : BaseSystem
    {
        readonly Family _chasingEnemyFamily = Family.All(typeof(MovementComponent), typeof(ChasingEnemyComponent)).Get();

        readonly ImmutableList<Entity> _chasingEnemyEntities;

        public ChasingSpeedIncreaseSystem(Engine engine) : base(engine)
        {
            _chasingEnemyEntities = engine.GetEntitiesFor(_chasingEnemyFamily);
        }

        public void Update(float dt)
        {
            foreach(Entity entity in _chasingEnemyEntities)
            {
                MovementComponent movementComp = entity.GetComponent<MovementComponent>();

                float speed = movementComp.MovementVector.Length();
                speed += CVars.Get<float>("chasing_enemy_acceleration") * dt;

                movementComp.MovementVector.Normalize();
                movementComp.MovementVector *= speed;
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
