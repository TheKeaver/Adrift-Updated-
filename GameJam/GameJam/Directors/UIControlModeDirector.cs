using Events;
using GameJam.Events.InputHandling;
using GameJam.Events.UI;

namespace GameJam.Directors
{
    public class UIControlModeDirector : BaseDirector
    {
        public UIControlModeDirector() : base(null, null, null)
        {
            IsPausable = false;
        }

        public override bool Handle(IEvent evt)
        {
            if(CVars.Get<bool>("ui_auto_control_mode_switching"))
            {
                GamePadButtonDownEvent gamePadButtonDownEvent = evt as GamePadButtonDownEvent;
                if(gamePadButtonDownEvent != null)
                {
                    if(CVars.Get<bool>("ui_mouse_mode"))
                    {
                        CVars.Get<bool>("ui_mouse_mode") = false;
                        CVars.Get<int>("ui_gamepad_mode_current_operator") = (int)gamePadButtonDownEvent._playerIndex;
                        EventManager.Instance.QueueEvent(new EnterGamePadUIModeEvent(gamePadButtonDownEvent._playerIndex));
                        EventManager.Instance.QueueEvent(new GamePadUIModeOperatorChangedEvent(gamePadButtonDownEvent._playerIndex));
                        return true;
                    }
                    if((int)gamePadButtonDownEvent._playerIndex != CVars.Get<int>("ui_gamepad_mode_current_operator"))
                    {
                        CVars.Get<int>("ui_gamepad_mode_current_operator") = (int)gamePadButtonDownEvent._playerIndex;
                        EventManager.Instance.QueueEvent(new GamePadUIModeOperatorChangedEvent(gamePadButtonDownEvent._playerIndex));
                    }
                }
                if (evt is KeyboardKeyDownEvent
                    || evt is MouseButtonEvent
                    || evt is MouseMoveEvent)
                {
                    if (!CVars.Get<bool>("ui_mouse_mode"))
                    {
                        CVars.Get<bool>("ui_mouse_mode") = true;
                        EventManager.Instance.QueueEvent(new EnterMouseUIModeEvent());
                    }
                }
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<MouseButtonEvent>(this);
            EventManager.Instance.RegisterListener<MouseMoveEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
