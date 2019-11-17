using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class ControllerInputMethod : InputMethod, IEventListener
    {
        public bool isRotatingRight;
        public bool isRotatingLeft;
        public readonly PlayerIndex PlayerIndex;
        readonly GamePadCapabilities _capabilities;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            _capabilities = GamePad.GetCapabilities(PlayerIndex);
            isRotatingLeft = false;
            isRotatingRight = false;

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonUpEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyUpEvent>(this);
        }

        public override void Update(float dt)
        {
            if(_capabilities.IsConnected)
            {
                if(isRotatingRight)
                {
                    // Counter-Clockwise
                    _snapshot.Angle -= CVars.Get<float>("input_shield_angular_speed") * dt;
                }
                if (isRotatingLeft)
                {
                    // Clockwise
                    _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
                }
            }
        }

        public bool Handle(IEvent evt)
        {
            GamePadButtonDownEvent gpbde = evt as GamePadButtonDownEvent;
            //if( gpbde._pressedButton == CVars.Get<Buttons>(""))
            GamePadButtonUpEvent gpbue = evt as GamePadButtonUpEvent;
            //if( gpbue._releasedButton == Carvs.Get<Buttons>(""))
            KeyboardKeyDownEvent kbkde = evt as KeyboardKeyDownEvent;
            //if( kbkde._pressedKey == CVars.Get<Keys>(""))
            KeyboardKeyUpEvent kbkue = evt as KeyboardKeyUpEvent;
            //if( kbkue._releasedKey == CVars.Get<Keys>(""))
            return false;
        }

        ~ControllerInputMethod()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}