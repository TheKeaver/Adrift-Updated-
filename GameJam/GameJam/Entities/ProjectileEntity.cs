using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public static class ProjectileEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Vector2 direction)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.PROJECTILE_BOUNDS));
            entity.AddComponent(new ProjectileComponent(3));
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new MovementComponent(direction, 30.0f));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new CollisionComponent(new Common.BoundingRect(0, 0, Constants.ObjectBounds.PROJECTILE_BOUNDS.X, Constants.ObjectBounds.PROJECTILE_BOUNDS.Y)));

            return entity;
        }
    }
}
