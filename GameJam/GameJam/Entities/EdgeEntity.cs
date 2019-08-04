using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public static class EdgeEntity
    {
        public static Entity Create(Engine engine, Vector2 position, Vector2 bounds, Vector2 normal)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new CollisionComponent(new BoundingRect(
                0, 0,
                bounds.X,
                bounds.Y
                )));
            entity.AddComponent(new EdgeComponent(normal));

            return entity;
        }
    }
}
