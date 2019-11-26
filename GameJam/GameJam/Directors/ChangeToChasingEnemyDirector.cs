using System;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.EnemyActions;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class ChangeToChasingEnemyDirector : BaseDirector
    {
        public ChangeToChasingEnemyDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<OutOfAmmoEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if (evt is OutOfAmmoEvent)
            {
                HandleOutOfAmmo(evt as OutOfAmmoEvent);
            }
            return false;
        }

        void HandleOutOfAmmo(OutOfAmmoEvent outOf)
        {
            outOf.ShootingEnemyOOA.GetComponent<MovementComponent>().MovementVector = CVars.Get<float>("chasing_enemy_speed") * (new Microsoft.Xna.Framework.Vector2((float)Math.Cos(outOf.ShootingEnemyOOA.GetComponent<TransformComponent>().Rotation), (float)Math.Sin(outOf.ShootingEnemyOOA.GetComponent<TransformComponent>().Rotation)));
            outOf.ShootingEnemyOOA.GetComponent<ProjectileSpawningProcessComponent>().FiringProcess.Kill();
            outOf.ShootingEnemyOOA.RemoveComponent<ProjectileSpawningProcessComponent>();
            outOf.ShootingEnemyOOA.RemoveComponent<ShootingEnemyComponent>();
            outOf.ShootingEnemyOOA.AddComponent(new ChasingEnemyComponent());
        }
    }
}
