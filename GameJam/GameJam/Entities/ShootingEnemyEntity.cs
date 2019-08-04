using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Processes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public static class ShootingEnemyEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, ProcessManager processManager, ContentManager conTENt)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.SHOOTING_SHIP_BOUNDS));
            entity.AddComponent(new ShootingEnemyComponent(1));
            entity.AddComponent(new RotationComponent(3.0f));
            entity.AddComponent(new MovementComponent());
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new CollisionComponent(new BoundingRect(0, 0, Constants.ObjectBounds.SHOOTING_SHIP_BOUNDS.X, Constants.ObjectBounds.SHOOTING_SHIP_BOUNDS.Y)));

            FireProjectileProcess fpp = new FireProjectileProcess(entity, engine, conTENt);
            processManager.Attach(fpp);
            entity.AddComponent(new ProjectileSpawningProcessComponent(fpp));

            return entity;
        }
    }
}
