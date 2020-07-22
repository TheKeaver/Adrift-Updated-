using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class TransformHistorySystem : BaseSystem
    {
        readonly Family _historyFamily = Family.All(typeof(TransformComponent), typeof(TransformHistoryComponent)).Get();
        readonly ImmutableList<Entity> _historyEntities;

        public TransformHistorySystem(Engine engine) : base(engine)
        {
            _historyEntities = engine.GetEntitiesFor(_historyFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity historyEntity in _historyEntities)
            {
                TransformHistoryComponent transformHistory = historyEntity.GetComponent<TransformHistoryComponent>();
                TransformComponent transform = historyEntity.GetComponent<TransformComponent>();

                transformHistory.AddToTransformHistory(new Vector2(transform.Position.X, transform.Position.Y), transform.Rotation);
            }
        }
    }
}
