﻿using Microsoft.Xna.Framework;
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

                if(currentState.IsButtonDown((Buttons)CVars.Get<int>("input_controller_clockwise")))
                {
                    // Counter-Clockwise
                    _snapshot.Angle -= CVars.Get<float>("input_shield_angular_speed") * dt;
                }
                if (currentState.IsButtonDown((Buttons)CVars.Get<int>("input_controller_counter_clockwise")))
                {
                    // Clockwise
                    _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
                }
            }
        }
    }
}