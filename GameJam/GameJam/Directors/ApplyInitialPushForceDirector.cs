using Audrey;
using Events;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class ApplyInitialPushForceDirector : BaseDirector
    {
        readonly Family _playerShipFamily = Family.All(typeof(TransformComponent), typeof(MovementComponent), typeof(PlayerShipComponent)).Get();

        readonly ImmutableList<Entity> _playerShipEntities;

        public ApplyInitialPushForceDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
            _playerShipEntities = engine.GetEntitiesFor(_playerShipFamily);
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is ApplyInitialPushForceEvent)
            {
                ApplyInitialPushForce();
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ApplyInitialPushForceEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
        
        private void ApplyInitialPushForce()
        {
            foreach(Entity entity in _playerShipEntities)
            {
                Vector2 direction = entity.GetComponent<TransformComponent>().Position;
                direction.Normalize();
                entity.GetComponent<MovementComponent>().MovementVector = direction * CVars.Get<float>("player_multiplayer_initial_push_force");
            }

            // Spawn entity
            PushForceEntity.Create(Engine, ProcessManager, Content, Vector2.Zero);
        }
    }
}
