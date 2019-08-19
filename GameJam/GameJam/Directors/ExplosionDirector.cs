using System;
using Audrey;
using Events;
using GameJam.Entities;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Directors
{
    public class ExplosionDirector : BaseDirector
    {
        public ExplosionDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CreateExplosionEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is CreateExplosionEvent)
            {
                CreateExplosionEvent createExplosionEvent = evt as CreateExplosionEvent;
                CreateExplosionEvent(createExplosionEvent.explosionLocation);
            }
            return false;
        }

        private void CreateExplosionEvent(Vector2 explosionLocation)
        {
            ExplosionEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_explosion")),
                explosionLocation);
        }
    }
}
