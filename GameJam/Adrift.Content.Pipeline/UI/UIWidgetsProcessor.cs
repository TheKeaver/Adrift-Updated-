using System;
using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Adrift.Content.Pipeline
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
