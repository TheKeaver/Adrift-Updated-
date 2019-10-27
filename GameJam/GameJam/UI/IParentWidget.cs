using System.Collections;

namespace GameJam.UI
{
    public interface IParentWidget : IEnumerable
    {
        void Add(Widget widget);
        void Remove(Widget widget);

        ISelectableWidget FindSelectedWidget();
    }
}
