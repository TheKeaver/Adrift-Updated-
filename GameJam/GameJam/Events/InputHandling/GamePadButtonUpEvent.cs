using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Events.InputHandling
{
    public class GamePadButtonUpEvent: IEvent
    {
        public PlayerIndex _playerIndex;
        public Buttons _releasedButton;

        public GamePadButtonUpEvent(PlayerIndex playerIndex, Buttons releasedButton)
        {
            _playerIndex = playerIndex;
            _releasedButton = releasedButton;
        }
    }
}
