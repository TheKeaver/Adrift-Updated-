using Audrey;
using Events;
using GameJam.Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Directors
{
    public class PauseDirector : BaseDirector
    {
        public PauseDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            KeyboardKeyDownEvent keyboardKeyDownEvent = evt as KeyboardKeyDownEvent;
            if(keyboardKeyDownEvent != null)
            {
                if(keyboardKeyDownEvent.Key == (Keys)CVars.Get<int>("input_keyboard_pause"))
                {
                    EventManager.Instance.QueueEvent(new TogglePauseGameEvent());
                    return true;
                }
            }

            GamePadButtonDownEvent gamePadButtonDownEvent = evt as GamePadButtonDownEvent;
            if(gamePadButtonDownEvent != null)
            {
                if(gamePadButtonDownEvent._pressedButton == (Buttons)CVars.Get<int>("input_controller_pause"))
                {
                    EventManager.Instance.QueueEvent(new TogglePauseGameEvent());
                    return true;
                }
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
