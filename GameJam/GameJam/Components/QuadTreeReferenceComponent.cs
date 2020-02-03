using Audrey;
using GameJam.Common;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class QuadTreeReferenceComponent : IComponent
    {
        public QuadTreeNode node;

        public QuadTreeReferenceComponent(QuadTreeNode n)
        {
            node = n;
        }
    }
}
