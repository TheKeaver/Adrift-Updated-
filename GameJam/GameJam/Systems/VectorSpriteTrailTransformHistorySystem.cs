using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;

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

                Vector2 ret1 = transformHistory.AddToTransformHistory(new Vector2(
                    transform.Position.X, transform.Position.Y), transform.Rotation);

                // According to my calculations "ret1" and "ret2" should match "Zero" and "One"
                Console.WriteLine("ret1: " + ret1);
            }
        }
    }
}
