using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class PrimaryKeyboardInputMethod : InputMethod
    {
        public override void Update(float dt)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                // Clockwise
                _snapshot.Angle -= Constants.Input.KEYBOARD_SHIELD_ANGULAR_SPEED * dt;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                // Counter-clockwise
                _snapshot.Angle += Constants.Input.KEYBOARD_SHIELD_ANGULAR_SPEED * dt;
            }
        }
    }
}
