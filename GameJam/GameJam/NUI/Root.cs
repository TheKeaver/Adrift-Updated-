using Audrey;
using Events;
using GameJam.Events;
using GameJam.NUI.Widgets;

namespace GameJam.NUI
{
    public class Root : TableWidget, IEventListener
    {

        public Root(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvent = evt as ResizeEvent;
            if(resizeEvent != null)
            {
                OnResize(resizeEvent.Width, resizeEvent.Height);
            }

            return false;
        }

        private void OnResize(float width, float height)
        {
            Width = new FixedValue<float>(width);
            Height = new FixedValue<float>(height);
        }
    }
}
