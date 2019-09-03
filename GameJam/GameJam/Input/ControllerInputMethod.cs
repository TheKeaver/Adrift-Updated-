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

                Vector2 stick;
                switch(CVars.Get<int>("controller_thumbstick"))
                {
                    case 0:
                        stick = currentState.ThumbSticks.Left;
                        break;
                    case 1:
                    default:
                        stick = currentState.ThumbSticks.Right;
                        break;
                }

                if(stick.LengthSquared() >= Math.Pow(CVars.Get<float>("controller_deadzone"), 2))
                {
                    // Stick is actually being pressed in a direction
                    _snapshot.Angle = (float)Math.Atan2(stick.Y, stick.X);
                }
            }
        }
    }
}