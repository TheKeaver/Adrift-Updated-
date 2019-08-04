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
    public class BulletBounceListenerDirector : BaseDirector
    {
        public BulletBounceListenerDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ProjectileBouncedEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if (evt is ProjectileBouncedEvent)
            {
                HandleBulletBounce(evt as ProjectileBouncedEvent);
            }
            return false;
        }

        void HandleBulletBounce(ProjectileBouncedEvent evt)
        {
            evt.projectile.GetComponent<ProjectileComponent>().bouncesLeft -= 1;

            if (evt.projectile.GetComponent<ProjectileComponent>().bouncesLeft <= 0)
                Engine.DestroyEntity(evt.projectile);
        }
    }
}
