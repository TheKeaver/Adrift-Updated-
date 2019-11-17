using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class ControllerInputMethod : InputMethod
    {
        public readonly PlayerIndex PlayerIndex;
        public string toString;
        readonly GamePadCapabilities _capabilities;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            toString = "controller_" + ((int)playerIndex);
            _capabilities = GamePad.GetCapabilities(PlayerIndex);
        }

        public override void Update(float dt)
        {
            if(_capabilities.IsConnected)
            {
                GamePadState currentState = GamePad.GetState(PlayerIndex);

                if(currentState.IsButtonDown((Buttons)CVars.Get<int>(toString + "_rotate_left")))
                {
                    // Counter-Clockwise
                    _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
                }
                if (currentState.IsButtonDown((Buttons)CVars.Get<int>(toString + "_rotate_right")))
                {
                    // Clockwise
                    _snapshot.Angle -= CVars.Get<float>("input_shield_angular_speed") * dt;
                }
            }
        }

        public override string ToString()
        {
            return toString;
        }
    }
}