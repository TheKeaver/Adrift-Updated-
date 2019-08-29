using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class CollisionComponent : IComponent
    {
        public BoundingRect BoundingBoxComponent;
        public List<Entity> CollidingWith = new List<Entity>();

        public CollisionComponent(BoundingRect newBoundRect)
        {
            BoundingBoxComponent = newBoundRect;
        }
    }
}
