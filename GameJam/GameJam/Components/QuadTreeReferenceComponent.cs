using Audrey;
using GameJam.Common;
using System;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class QuadTreeReferenceComponent : IComponent, ICopyComponent
    {
        public QuadTreeNode node;

        public QuadTreeReferenceComponent(QuadTreeNode n)
        {
            node = n;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            // The QuadTree will be remade next frame anyways
            return new QuadTreeReferenceComponent(null);
        }
    }
}
