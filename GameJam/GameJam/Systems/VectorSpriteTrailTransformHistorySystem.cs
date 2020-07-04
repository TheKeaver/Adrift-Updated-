using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    /*
     * This system is responsible for updating each entity with a "VectorSpriteTrailComponent"
     */
    public class VectorSpriteTrailTransformHistorySystem : BaseSystem
    {
        readonly Family _historyFamily = Family.All(typeof(VectorSpriteTrailComponent), typeof(TransformComponent), typeof(TransformHistoryComponent)).Get();
        readonly ImmutableList<Entity> _historyEntities;

        public VectorSpriteTrailTransformHistorySystem(Engine engine) : base(engine)
        {
            _historyEntities = engine.GetEntitiesFor(_historyFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity historyEntity in _historyEntities)
            {
                TransformHistoryComponent transformHistory = historyEntity.GetComponent<TransformHistoryComponent>();
                TransformComponent transform = historyEntity.GetComponent<TransformComponent>();

                transformHistory.AddToTransformHistory(new Vector2(
                    transform.Position.X - 2, transform.Position.Y), transform.Rotation);
                transformHistory.AddToTransformHistory(new Vector2(
                    transform.Position.X + 2, transform.Position.Y), transform.Rotation);
            }
        }
    }
}
