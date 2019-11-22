using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Input
{
    public class ControllerInputMethod : InputMethod, IEventListener
    {
        public bool isRotatingCW;
        public bool isRotatingCCW;
        public readonly PlayerIndex PlayerIndex;
        readonly GamePadCapabilities _capabilities;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            _capabilities = GamePad.GetCapabilities(PlayerIndex);
            isRotatingCCW = false;
            isRotatingCW = false;

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonUpEvent>(this);
        }

        public override void Update(float dt)
        {
            if(_capabilities.IsConnected)
            {
                if(isRotatingCW)
                {
                    // Counter-Clockwise
                    _snapshot.Angle -= CVars.Get<float>("input_shield_angular_speed") * dt;
                }
                if (isRotatingCCW)
                {
                    // Clockwise
                    _snapshot.Angle += CVars.Get<float>("input_shield_angular_speed") * dt;
                }
            }
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GamePadButtonDownEvent)
                HandleGamePadRotationOn(evt as GamePadButtonDownEvent);
            if (evt is GamePadButtonUpEvent)
                HandleGamePadRotationOff(evt as GamePadButtonUpEvent);

            return false;
        }

        private void HandleGamePadRotationOn(GamePadButtonDownEvent gpbde)
        {
            if ((int)gpbde._pressedButton == CVars.Get<int>("controller_" + ((int)gpbde._playerIndex) + "_rotate_left"))
            {
                isRotatingCCW = true;
            }
            if ((int)gpbde._pressedButton == CVars.Get<int>("controller_" + ((int)gpbde._playerIndex) + "_rotate_right"))
            {
                isRotatingCW = true;
            }
        }

        private void HandleGamePadRotationOff(GamePadButtonUpEvent gpbue)
        {
            if ((int)gpbue._releasedButton == CVars.Get<int>("controller_" + ((int)gpbue._playerIndex) + "_rotate_left"))
            {
                isRotatingCCW = false;
            }
            if ((int)gpbue._releasedButton == CVars.Get<int>("controller_" + ((int)gpbue._playerIndex) + "_rotate_right"))
            {
                isRotatingCW = false;
            }
        }

        ~ControllerInputMethod()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}