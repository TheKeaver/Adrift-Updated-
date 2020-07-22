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

        public ControllerInputMethod(PlayerIndex playerIndex, bool isSuperShieldOn = false)
        {
            PlayerIndex = playerIndex;
            isRotatingCCW = false;
            isRotatingCW = false;

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonUpEvent>(this);
            EventManager.Instance.RegisterListener<GamePadDisconnectedEvent>(this);
            this.isSuperShieldOn = isSuperShieldOn;
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

            if ((int)gpbde._pressedButton == CVars.Get<int>("input_controller_" + ((int)gpbde._playerIndex) + "_rotate_counter_clockwise"))
            {
                isRotatingCCW = true;
            }
            if ((int)gpbde._pressedButton == CVars.Get<int>("input_controller_" + ((int)gpbde._playerIndex) + "_rotate_clockwise"))
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

            if((int)gpbde._pressedButton == CVars.Get<int>("input_controller_" + ((int)gpbde._playerIndex) + "_super_shield"))
            {
                _snapshot.SuperShield = true;
            }
        }

        private void HandleGamePadRotationOff(GamePadButtonUpEvent gpbue)
        {
            if(gpbue._playerIndex != PlayerIndex)
            {
                return;
            }

            if ((int)gpbue._releasedButton == CVars.Get<int>("input_controller_" + ((int)gpbue._playerIndex) + "_rotate_counter_clockwise"))
            {
                isRotatingCCW = false;
            }
            if ((int)gpbue._releasedButton == CVars.Get<int>("input_controller_" + ((int)gpbue._playerIndex) + "_rotate_clockwise"))
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

            if ((int)gpbue._releasedButton == CVars.Get<int>("input_controller_" + ((int)gpbue._playerIndex) + "_super_shield"))
            {
                _snapshot.SuperShield = false;
            }
        }

        private void HandleGamePadDisconnected(GamePadDisconnectedEvent gpde)
        {
            isRotatingCCW = isRotatingCW = false;
        }

        public override InputMethod Copy()
        {
            InputMethod copy = new ControllerInputMethod(PlayerIndex, isSuperShieldOn);
            copy.GetSnapshot().Angle = _snapshot.Angle;
            copy.GetSnapshot().SuperShield = _snapshot.SuperShield;

            return copy;
        }

        ~ControllerInputMethod()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}