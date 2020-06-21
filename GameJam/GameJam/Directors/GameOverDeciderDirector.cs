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
            EventManager.Instance.RegisterListener<PlayerLostEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void HandlePlayerLostEvent(PlayerLostEvent playerLostEvent)
        {
            if(_playerShipEntities.Count == 0
                || !CVars.Get<bool>("player_individual_deaths"))
            {
                EventManager.Instance.QueueEvent(new GameOverEvent());
            }
        }
    }
}
