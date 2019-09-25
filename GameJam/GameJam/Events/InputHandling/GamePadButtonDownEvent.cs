using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    class GamePadButtonDownEvent : IEvent
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
