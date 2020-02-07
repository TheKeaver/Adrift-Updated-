using Audrey;
using GameJam.Common;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class QuadTreeSystem : BaseSystem
    {
        readonly Family _quadTreeFamily = Family.All(typeof(QuadTreeReferenceComponent)).Get();
        readonly ImmutableList<Entity> _quadTreeEntities;

        public QuadTreeSystem(Engine engine) : base(engine)
        {
            _quadTreeEntities = engine.GetEntitiesFor(_quadTreeFamily);
        }

        public override void Update(float dt)
        {
            ConstructQuadTree();
        }

        private void ConstructQuadTree()
        {
            float width = CVars.Get<float>("play_field_width");
            float height = CVars.Get<float>("play_field_height");
            QuadTreeNode qt = new QuadTreeNode(new BoundingRect(-width/2, -height/2, width, height), null);

            foreach (Entity e in _quadTreeEntities)
            {
                qt.AddReference(e);
            }
        }
    }
}
