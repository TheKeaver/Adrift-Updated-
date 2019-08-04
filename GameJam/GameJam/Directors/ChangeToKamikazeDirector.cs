using System;
using System.Collections.Generic;
using System.Text;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class ChangeToKamikazeDirector : BaseDirector
    {
        public ChangeToKamikazeDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<OutOfAmmoEvent>(this);
        }

        public override void UnregisterEvents()
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
            outOf.shootingEnemyOOA.GetComponent<MovementComponent>().speed = Constants.GamePlay.KAMIKAZE_ENEMY_SPEED;
            outOf.shootingEnemyOOA.GetComponent<ProjectileSpawningProcessComponent>().firingProcess.Kill();
            outOf.shootingEnemyOOA.RemoveComponent<ProjectileSpawningProcessComponent>();
            outOf.shootingEnemyOOA.RemoveComponent<ShootingEnemyComponent>();
            outOf.shootingEnemyOOA.AddComponent(new KamikazeComponent());
        }
    }
}
