using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class PrimaryKeyboardInputMethod : InputMethod
    {
        public override void Update(float dt)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                // Clockwise
                _snapshot.Angle -= Constants.Input.KEYBOARD_SHIELD_ANGULAR_SPEED * dt;
            }
            if(keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                // Counter-clockwise
                _snapshot.Angle += Constants.Input.KEYBOARD_SHIELD_ANGULAR_SPEED * dt;
            }
        }
    }
}
