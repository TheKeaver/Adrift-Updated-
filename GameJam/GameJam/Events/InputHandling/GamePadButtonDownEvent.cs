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
        public int _playerIndex;
        public int _pressedButton;

        public GamePadButtonDownEvent(PlayerIndex playerIndex, Buttons pressedButton)
        {
            _playerIndex = (int)playerIndex;
            _pressedButton = (int)pressedButton;
        }
    }
}
