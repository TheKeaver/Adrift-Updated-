using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Events.InputHandling
{
    public class GamePadButtonDownEvent : IEvent
    {
        public PlayerIndex _playerIndex;
        public Buttons _pressedButton;

        public GamePadButtonDownEvent(PlayerIndex playerIndex, Buttons pressedButton)
        {
            _pressedButton = pressedButton;
            _playerIndex = playerIndex;
        }
    }
}
