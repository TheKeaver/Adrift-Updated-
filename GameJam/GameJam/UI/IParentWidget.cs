using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using UI.Content.Pipeline;

namespace GameJam.UI
{
    public interface IParentWidget : IEnumerable
    {
        void Add(Widget widget);
        void Remove(Widget widget);

        void BuildFromPrototypes(ContentManager content, List<WidgetPrototype> prototypes);

        ISelectableWidget FindSelectedWidget();
        List<Widget> FindWidgetsByClass(string className);
    }
}
