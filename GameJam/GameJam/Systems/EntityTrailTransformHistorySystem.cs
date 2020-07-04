using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    /*
     * EntityTrailComponent = Flag component, no data stored
     * This system just updates the entity's TrasnformHistory every update
     */
    public class EntityTrailTransformHistorySystem : BaseSystem
    {
        readonly Family _historyFamily = Family.All(typeof(FadingEntityComponent), typeof(TransformComponent), typeof(TransformHistoryComponent)).Get();
        readonly ImmutableList<Entity> _historyEntities;

        public EntityTrailTransformHistorySystem(Engine engine) : base(engine)
        {
            _historyEntities = engine.GetEntitiesFor(_historyFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity historyEntity in _historyEntities)
            {
                TransformHistoryComponent transformHistory = historyEntity.GetComponent<TransformHistoryComponent>();
                TransformComponent transform = historyEntity.GetComponent<TransformComponent>();

                transformHistory.AddToTransformHistory(transform.Position, transform.Rotation);
            }
        }
    }
}
