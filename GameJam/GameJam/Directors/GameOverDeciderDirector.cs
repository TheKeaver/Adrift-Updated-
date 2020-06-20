using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class GameOverDeciderDirector : BaseDirector
    {
        readonly Family _playerShipFamily = Family.All(typeof(PlayerShipComponent)).Get();
        readonly ImmutableList<Entity> _playerShipEntities;

        public GameOverDeciderDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
            _playerShipEntities = Engine.GetEntitiesFor(_playerShipFamily);
        }

        public override bool Handle(IEvent evt)
        {
            PlayerLostEvent playerLostEvent = evt as PlayerLostEvent;
            if(playerLostEvent != null)
            {
                HandlePlayerLostEvent(playerLostEvent);
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            // TODO: Player destroyed
            EventManager.Instance.RegisterListener<PlayerLostEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void HandlePlayerLostEvent(PlayerLostEvent playerLostEvent)
        {
            // TODO: Add animation

            // Have a director handle player lost animation? Listen for this same event
            // Then this director dispatches a GameOverEvent

            if(_playerShipEntities.Count == 0)
            {
                EventManager.Instance.QueueEvent(new GameOverEvent());
            }
        }
    }
}
