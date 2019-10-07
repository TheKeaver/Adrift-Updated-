using Audrey;
using Events;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Events;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Processes
{
    public class FireProjectileProcess : IntervalProcess
    {
        public Entity ShootingEnemy;
        public Engine Engine;
        ContentManager Content;
        public FireProjectileProcess(Entity shootingEnemy, Engine engine, ContentManager content) : base(3)
        {
            this.ShootingEnemy = shootingEnemy;
            this.Engine = engine;
            Content = content;
        }

        protected override void OnTick(float interval)
        {
            if (ShootingEnemy.GetComponent<ShootingEnemyComponent>().AmmoLeft <= 0)
            {
                EventManager.Instance.QueueEvent(new OutOfAmmoEvent(ShootingEnemy));
                return;
            }
            if (!Engine.GetEntities().Contains(ShootingEnemy))
            {
                Kill();
                return;
            }

            ProjectileEntity.Create(Engine, Content.Load<Texture2D>(CVars.Get<string>("texture_enemy_bullet")), ShootingEnemy.GetComponent<TransformComponent>().Position, new Microsoft.Xna.Framework.Vector2((float)Math.Cos(ShootingEnemy.GetComponent<TransformComponent>().Rotation),(float)Math.Sin(ShootingEnemy.GetComponent<TransformComponent>().Rotation)));
            EventManager.Instance.QueueEvent(new ProjectileFiredEvent());
            ShootingEnemy.GetComponent<ShootingEnemyComponent>().AmmoLeft -= 1;
        }
    }
}
