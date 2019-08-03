using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class ControllerInputMethod : InputMethod
    {
        public readonly PlayerIndex PlayerIndex;
        readonly GamePadCapabilities _capabilities;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            _capabilities = GamePad.GetCapabilities(PlayerIndex);
        }

        public override void Update(float dt)
        {
            if(_capabilities.IsConnected)
            {
                GamePadState currentState = GamePad.GetState(PlayerIndex);

                Vector2 rightStick = currentState.ThumbSticks.Right;

                if(rightStick.LengthSquared() >= Math.Pow(Constants.Input.DEADZONE, 2))
                {
                    // Stick is actually being pressed in a direction
                    _snapshot.Angle = (float)Math.Atan2(rightStick.Y, rightStick.X);
                }
            }
        }
    }
}