using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace UI.Content.Pipeline
{
    [ContentProcessor(DisplayName = "UI Widgets Processor - Adrift")]
    public class UIWidgetsProcessor : ContentProcessor<UIWidgetsFile, List<WidgetPrototype>>
    {
        public override List<WidgetPrototype> Process(UIWidgetsFile input, ContentProcessorContext context)
        {
            return input.Widgets;
        }
    }
}
