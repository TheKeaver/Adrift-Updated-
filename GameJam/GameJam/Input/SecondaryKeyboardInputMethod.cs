using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameJam.Input
{
    public class SecondaryKeyboardInputMethod : InputMethod, IEventListener
    {
        public bool isRotatingCW;
        public bool isRotatingCCW;

        public SecondaryKeyboardInputMethod()
        {
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyUpEvent>(this);
            isRotatingCCW = false;
            isRotatingCW = false;
        }

        public bool Handle(IEvent evt)
        {
            if(evt is KeyboardKeyDownEvent)
                HandleKeyboardControlsPressed(evt as KeyboardKeyDownEvent);

            if (evt is KeyboardKeyUpEvent)
                HandleKeyboardControlsReleased(evt as KeyboardKeyUpEvent);

            return false;
        }

        private void HandleKeyboardControlsReleased(KeyboardKeyUpEvent keyboardKeyUpEvent)
        {
            if ((int)keyboardKeyUpEvent._key == CVars.Get<int>("input_keyboard_secondary_rotate_counter_clockwise"))
            {
                isRotatingCCW = false;
            }
            if ((int)keyboardKeyUpEvent._key == CVars.Get<int>("input_keyboard_secondary_rotate_clockwise"))
            {
                isRotatingCW = false;
            }
            if ((int)keyboardKeyUpEvent._key == CVars.Get<int>("input_keyboard_secondary_super_shield"))
            {
                _snapshot.SuperShield = false;
            }
        }

        private void HandleKeyboardControlsPressed(KeyboardKeyDownEvent keyboardKeyDownEvent)
        {
            if ((int)keyboardKeyDownEvent._key == CVars.Get<int>("input_keyboard_secondary_rotate_counter_clockwise"))
            {
                isRotatingCCW = true;
            }
            if ((int)keyboardKeyDownEvent._key == CVars.Get<int>("input_keyboard_secondary_rotate_clockwise"))
            {
                isRotatingCW = true;
            }
            if ((int)keyboardKeyDownEvent._key == CVars.Get<int>("input_keyboard_secondary_super_shield"))
            {
                _snapshot.SuperShield = true;
            }
        }

        public override void Update(float dt)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if(isRotatingCW)
            {
                // Clockwise
                _snapshot.Angle -= CVars.Get<float>("input_shield_angular_speed") * dt;
            }
            if (isRotatingCCW)
            {
                // Counter-clockwise
                _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
            }
        }

        public override InputMethod Copy()
        {
            InputMethod copy = new SecondaryKeyboardInputMethod();
            copy.GetSnapshot().Angle = _snapshot.Angle;
            copy.GetSnapshot().SuperShield = _snapshot.SuperShield;

            return copy;
        }

        ~SecondaryKeyboardInputMethod()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
