using Audrey;
using Events;
using GameJam.Events.Animation;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Directors
{
    class PlayerLostAnimationAttachDirector : BaseDirector
    {
        public PlayerLostAnimationAttachDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            PlayerLostEvent playerLostEvent = evt as PlayerLostEvent;
            if(playerLostEvent != null)
            {
                // TODO
                EventManager.Instance.QueueEvent(new PlayerLostAnimationCompletedEvent(playerLostEvent.Player));
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<PlayerLostEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
