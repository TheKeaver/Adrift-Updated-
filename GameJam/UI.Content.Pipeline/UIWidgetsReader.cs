using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Content;

namespace UI.Content.Pipeline
{
    public class UIWidgetsReader : ContentTypeReader<List<WidgetPrototype>>
    {
        protected override List<WidgetPrototype> Read(ContentReader input, List<WidgetPrototype> existingInstance)
        {
            int count = input.ReadInt32();
            List<WidgetPrototype> widgets = new List<WidgetPrototype>();

            for(int i = 0; i < count; i++)
            {
                string assemblyQualifiedName = input.ReadString();
                WidgetPrototype widget = (WidgetPrototype)Activator.CreateInstance(Type.GetType(assemblyQualifiedName));
                widget.ReadFromInput(input);
                widgets.Add(widget);
            }

            return widgets;
        }
    }
}
