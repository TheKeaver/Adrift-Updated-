using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameJam.Input
{
    public class PrimaryKeyboardInputMethod : InputMethod, IEventListener
    {
        public bool isRotatingCW;
        public bool isRotatingCCW;
        public int rotatingValue; // -1 : CCW, 0 : None, 1 : CW

        public PrimaryKeyboardInputMethod()
        {
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyUpEvent>(this);
            isRotatingCCW = false;
            isRotatingCW = false;
            rotatingValue = 0;
        }

        public bool Handle(IEvent evt)
        {
            if (evt is KeyboardKeyDownEvent)
                HandleKeyboardControlsPressed(evt as KeyboardKeyDownEvent);

            if (evt is KeyboardKeyUpEvent)
                HandleKeyboardControlsReleased(evt as KeyboardKeyUpEvent);

            return false;
        }

        private void HandleKeyboardControlsReleased(KeyboardKeyUpEvent keyboardKeyUpEvent)
        {
            if( (int)keyboardKeyUpEvent._key == CVars.Get<int>("input_keyboard_primary_rotate_counter_clockwise"))
            {
                isRotatingCCW = false;

            }
            if((int)keyboardKeyUpEvent._key == CVars.Get<int>("input_keyboard_primary_rotate_clockwise"))
            {
                isRotatingCW = false;
            }
            if((int)keyboardKeyUpEvent._key == CVars.Get<int>("input_keyboard_primary_super_shield"))
            {
                _snapshot.SuperShield = false;
                Console.WriteLine("Super Shield False");
            }
        }

        private void HandleKeyboardControlsPressed(KeyboardKeyDownEvent keyboardKeyDownEvent)
        {
            if ((int)keyboardKeyDownEvent._key == CVars.Get<int>("input_keyboard_primary_rotate_counter_clockwise"))
            {
                isRotatingCCW = true;
            }
            if ((int)keyboardKeyDownEvent._key == CVars.Get<int>("input_keyboard_primary_rotate_clockwise"))
            {
                isRotatingCW = true;
            }
            if ((int)keyboardKeyDownEvent._key == CVars.Get<int>("input_keyboard_primary_super_shield"))
            {
                _snapshot.SuperShield = true;
                Console.WriteLine("Super Shield True");
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
            if(isRotatingCCW)
            {
                // Counter-clockwise
                _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
            }
        }
        ~PrimaryKeyboardInputMethod()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
