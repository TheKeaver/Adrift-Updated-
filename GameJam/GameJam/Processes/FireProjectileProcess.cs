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
        public Entity shootingEnemy;
        public Engine engine;
        ContentManager Content;
        public FireProjectileProcess(Entity shooter, Engine ngin, ContentManager conTENt) : base(3)
        {
            shootingEnemy = shooter;
            engine = ngin;
            Content = conTENt;
        }

        protected override void OnTick(float interval)
        {
            if (shootingEnemy.GetComponent<ShootingEnemyComponent>().ammoLeft <= 0)
            {
                EventManager.Instance.QueueEvent(new OutOfAmmoEvent(shootingEnemy));
                return;
            }
            if (!engine.GetEntities().Contains(shootingEnemy))
            {
                Kill();
                return;
            }
            ProjectileEntity.Create(engine, Content.Load<Texture2D>(Constants.Resources.TEXTURE_ENEMY_BULLET), shootingEnemy.GetComponent<TransformComponent>().Position, shootingEnemy.GetComponent<MovementComponent>().direction);
            EventManager.Instance.QueueEvent(new ProjectileFiredEvent());
            shootingEnemy.GetComponent<ShootingEnemyComponent>().ammoLeft -= 1;
            Console.WriteLine(shootingEnemy.GetComponent<ShootingEnemyComponent>().ammoLeft);
        }
    }
}
