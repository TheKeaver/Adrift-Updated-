using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    class CleanupEntitiesOnEndOfSceneDirector : BaseDirector
    {
        public CleanupEntitiesOnEndOfSceneDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is GameOverEvent)
            {
                CleanDestroyAllEntities(false);
            }
            if(evt is CleanupAllEntitiesEvent)
            {
                CleanDestroyAllEntities(true);
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<GameOverEvent>(this);
            EventManager.Instance.RegisterListener<CleanupAllEntitiesEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void CleanDestroyAllEntities(bool includeEdges = true)
        {
            // Explode all entities
            ImmutableList<Entity> explosionEntities = Engine.GetEntitiesFor(Family
                .All(typeof(TransformComponent), typeof(ColoredExplosionComponent))
                .Exclude(typeof(DontDestroyForGameOverComponent))
                .Get());
            foreach (Entity entity in explosionEntities)
            {
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                ColoredExplosionComponent coloredExplosionComp = entity.GetComponent<ColoredExplosionComponent>();
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(transformComp.Position,
                    coloredExplosionComp.Color,
                    false));
            }

            // Destroy all entities
            Engine.DestroyEntitiesFor(Family.Exclude(typeof(DontDestroyForGameOverComponent),
                typeof(ParallaxBackgroundComponent),
                typeof(EdgeComponent)).Get());

            // Destroy edges (if needed)
            if(includeEdges)
            {
                // Fade out edges
                foreach (Entity edgeEntity in Engine.GetEntitiesFor(Family.All(typeof(EdgeComponent), typeof(VectorSpriteComponent)).Get()))
                {
                    ProcessManager.Attach(new SpriteEntityFadeOutProcess(Engine, edgeEntity, CVars.Get<float>("game_over_edge_fade_out_duration"), Easings.Functions.QuadraticEaseOut))
                        .SetNext(new EntityDestructionProcess(Engine, edgeEntity));
                }
            }
        }
    }
}
