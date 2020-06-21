using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.Animation;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    class PlayerLostAnimationAttachDirector : BaseDirector
    {
        public PlayerLostAnimationAttachDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            PlayerLostEvent playerLostEvent = evt as PlayerLostEvent;
            if(playerLostEvent != null)
            {
                HandlePlayerLostEvent(playerLostEvent);
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<PlayerLostEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void HandlePlayerLostEvent(PlayerLostEvent playerLostEvent)
        {
            Entity responsibleEntity = playerLostEvent.ResponsibleEntity;

            responsibleEntity.StripAllComponentsExcept(typeof(TransformComponent),
                typeof(VectorSpriteComponent),
                typeof(SpriteComponent),
                typeof(ColoredExplosionComponent),
                typeof(DontDestroyForGameOverComponent));
            responsibleEntity.AddComponent(new DontDestroyForGameOverComponent());

            // Make the camera zoom to include the entity
            responsibleEntity.AddComponent(new CameraTrackingComponent());

            ProcessManager.Attach(new SpriteEntityFlashProcess(Engine, responsibleEntity, CVars.Get<int>("game_over_responsible_enemy_flash_count"), CVars.Get<float>("game_over_responsible_enemy_flash_period") / 2))
                .SetNext(new DelegateProcess(() =>
            {
                // Explosion
                TransformComponent transformComp = responsibleEntity.GetComponent<TransformComponent>();
                ColoredExplosionComponent coloredExplosionComp = responsibleEntity.GetComponent<ColoredExplosionComponent>();
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(transformComp.Position,
                    coloredExplosionComp.Color,
                    false));
            })).SetNext(new EntityDestructionProcess(Engine, responsibleEntity)).SetNext(new DelegateProcess(() =>
            {
                EventManager.Instance.QueueEvent(new PlayerLostAnimationCompletedEvent(playerLostEvent.Player));
            }));
        }
    }
}
