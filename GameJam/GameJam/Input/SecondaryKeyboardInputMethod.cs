using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class SecondaryKeyboardInputMethod : InputMethod
    {
        public override void Update(float dt)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if( keyboardState.IsKeyDown((Keys)CVars.Get<int>("input_keyboard_secondary_clockwise")))
            {
                // Clockwise
                _snapshot.Angle -= CVars.Get<float>("input_shield_angular_speed") * dt;
            }
            if (keyboardState.IsKeyDown((Keys)CVars.Get<int>("input_keyboard_secondary_counter_clockwise")))
            {
                // Counter-clockwise
                _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
            }
        }
    }
}
