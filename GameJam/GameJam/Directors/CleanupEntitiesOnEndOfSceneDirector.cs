using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
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
            FamilyBuilder familyBuilder = Family.Exclude(typeof(DontDestroyForGameOverComponent),
                typeof(ParallaxBackgroundComponent));
            if (!includeEdges)
            {
                familyBuilder.Exclude(typeof(EdgeComponent));
            }
            Engine.DestroyEntitiesFor(familyBuilder.Get());
        }
    }
}
