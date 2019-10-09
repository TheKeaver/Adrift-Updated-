using Audrey;
using Events;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Events;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes
{
    public class FireProjectileProcess : IntervalProcess
    {
        public Entity ShootingEnemy;
        public Engine Engine;

        private readonly float _shootingEnemyTip = 5;
        private readonly float _projectileLength = 3;
        private readonly float _errorBuffer = 0f;

        public FireProjectileProcess(Entity shootingEnemy, Engine engine) : base(3)
        {
            ShootingEnemy = shootingEnemy;
            Engine = engine;
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

            TransformComponent transformComp = ShootingEnemy.GetComponent<TransformComponent>();
            Vector2 shootingEnemyDirection = new Vector2((float)Math.Cos(ShootingEnemy.GetComponent<TransformComponent>().Rotation), (float)Math.Sin(ShootingEnemy.GetComponent<TransformComponent>().Rotation));
            Entity projectile = ProjectileEntity.Create(Engine, Vector2.Zero, shootingEnemyDirection);
            TransformComponent projectileTransformComp = projectile.GetComponent<TransformComponent>();
            Vector2 projectilePosition = shootingEnemyDirection * (_shootingEnemyTip * transformComp.Scale + _projectileLength * projectileTransformComp.Scale + _errorBuffer) + transformComp.Position;
            projectileTransformComp.SetPosition(projectilePosition);

            EventManager.Instance.QueueEvent(new ProjectileFiredEvent());
            ShootingEnemy.GetComponent<ShootingEnemyComponent>().AmmoLeft -= 1;
        }
    }
}
