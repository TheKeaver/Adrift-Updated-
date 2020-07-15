using Audrey;
using GameJam.Common;
using GameJam.Components;

namespace GameJam.Systems
{
    public class QuadTreeSystem : BaseSystem
    {
        readonly Family _quadTreeFamily = Family.All(typeof(QuadTreeReferenceComponent), typeof(TransformComponent), typeof(CollisionComponent)).Get();
        readonly ImmutableList<Entity> _quadTreeEntities;

        public QuadTreeSystem(Engine engine) : base(engine)
        {
            _quadTreeEntities = engine.GetEntitiesFor(_quadTreeFamily);
        }

        public void Update(float dt)
        {
            ConstructQuadTree();
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

        private void ConstructQuadTree()
        {
            float width = CVars.Get<float>("play_field_width");
            float height = CVars.Get<float>("play_field_height");
            QuadTreeNode qt = new QuadTreeNode(new BoundingRect(-width / 2, -height / 2, width, height), null);

            foreach (Entity e in _quadTreeEntities)
            {
                qt.AddReference(e);
            }
        }
    }
}
