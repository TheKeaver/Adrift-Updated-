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
        public bool isSuperShieldOn;
        public readonly PlayerIndex PlayerIndex;

        public ControllerInputMethod(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            isRotatingCCW = false;
            isRotatingCW = false;

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonUpEvent>(this);
            EventManager.Instance.RegisterListener<GamePadDisconnectedEvent>(this);
        }

        public override void Update(float dt)
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

        public bool Handle(IEvent evt)
        {
            if (evt is GamePadButtonDownEvent)
            {
                HandleGamePadRotationOn(evt as GamePadButtonDownEvent);
                HandleGamePadSuperShieldOn(evt as GamePadButtonDownEvent);
            }
            if (evt is GamePadButtonUpEvent)
            {
                HandleGamePadRotationOff(evt as GamePadButtonUpEvent);
                HandleGamePadSuperShieldOff(evt as GamePadButtonUpEvent);
            }

            if (evt is GamePadDisconnectedEvent)
                HandleGamePadDisconnected(evt as GamePadDisconnectedEvent);

            return false;
        }

        private void HandleGamePadRotationOn(GamePadButtonDownEvent gpbde)
        {
            if(gpbde._playerIndex != PlayerIndex)
            {
                return;
            }

            if ((int)gpbde._pressedButton == CVars.Get<int>("controller_" + ((int)gpbde._playerIndex) + "_rotate_left"))
            {
                isRotatingCCW = true;
            }
            if ((int)gpbde._pressedButton == CVars.Get<int>("controller_" + ((int)gpbde._playerIndex) + "_rotate_right"))
            {
                isRotatingCW = true;
            }
        }

        private void HandleGamePadSuperShieldOn(GamePadButtonDownEvent gpbde)
        {
            if (gpbde._playerIndex != PlayerIndex)
            {
                return;
            }

            if((int)gpbde._playerIndex == CVars.Get<int>("controller_" + ((int)gpbde._playerIndex) + "_super_shield"))
            {
                isSuperShieldOn = true;
            }
        }

        private void HandleGamePadRotationOff(GamePadButtonUpEvent gpbue)
        {
            if(gpbue._playerIndex != PlayerIndex)
            {
                return;
            }

            if ((int)gpbue._releasedButton == CVars.Get<int>("controller_" + ((int)gpbue._playerIndex) + "_rotate_left"))
            {
                isRotatingCCW = false;
            }
            if ((int)gpbue._releasedButton == CVars.Get<int>("controller_" + ((int)gpbue._playerIndex) + "_rotate_right"))
            {
                isRotatingCW = false;
            }
        }

        private void HandleGamePadSuperShieldOff(GamePadButtonUpEvent gpbue)
        {
            if (gpbue._playerIndex != PlayerIndex)
            {
                return;
            }

            if ((int)gpbue._playerIndex == CVars.Get<int>("controller_" + ((int)gpbue._playerIndex) + "_super_shield"))
            {
                isSuperShieldOn = false;
            }
        }

        private void HandleGamePadDisconnected(GamePadDisconnectedEvent gpde)
        {
            isRotatingCCW = isRotatingCW = false;
        }

        ~ControllerInputMethod()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}