using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public static class KamikazeEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.KAMIKAZE_SHIP_BOUNDS));
            entity.AddComponent(new RotationComponent(0.5f));
            entity.AddComponent(new MovementComponent());
            entity.GetComponent<MovementComponent>().speed = 20.0f;
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new CollisionComponent(new BoundingRect(0, 0, Constants.ObjectBounds.KAMIKAZE_SHIP_BOUNDS.X, Constants.ObjectBounds.KAMIKAZE_SHIP_BOUNDS.Y)));
            entity.AddComponent(new KamikazeComponent());

            return entity;
        }
    }
}
