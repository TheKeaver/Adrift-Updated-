using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Adrift.Content.Common.UI
{
    public class UIWidgetsReader : ContentTypeReader<List<WidgetPrototype>>
    {
        protected override List<WidgetPrototype> Read(ContentReader input, List<WidgetPrototype> existingInstance)
        {
            int count = input.ReadInt32();
            List<WidgetPrototype> widgets = new List<WidgetPrototype>();

            for(int i = 0; i < count; i++)
            {
                // Hack: content is built in a different project (Adrift.Content.Pipeline). The assembly qualified name
                // is different when writing than when loading. We aren't necessarily worried about what project
                // the class was from, we want to know how to instantiate it.
                // The alternative to this would be to make Adrift.Content.Common a library instead of a shared project,
                // however this is not ideal since we want that project to be able to be referenced by both .NET Framework
                // and .NET Core projects.
                string assemblyName = GetType().Assembly.FullName.Split(',')[0];
                string className = input.ReadString().Split(',')[0];
                string assemblyQualifiedName = string.Format("{0}, {1}", className, assemblyName);
                WidgetPrototype widget = (WidgetPrototype)Activator.CreateInstance(Type.GetType(assemblyQualifiedName));
                widget.ReadFromInput(input);
                widgets.Add(widget);
            }

            return widgets;
        }
    }
}
