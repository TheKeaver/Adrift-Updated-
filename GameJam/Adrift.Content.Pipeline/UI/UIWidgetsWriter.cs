using System;
using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Adrift.Content.Pipeline
{
    [ContentTypeWriter]
    public class UIWidgetsWriter : ContentTypeWriter<List<WidgetPrototype>>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Adrift.Content.Common.UI.UIWidgetsReader, GameJam";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(List<WidgetPrototype>).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, List<WidgetPrototype> value)
        {
            output.Write(value.Count);
            
            foreach (WidgetPrototype widget in value)
            {
                output.Write(widget.GetType().AssemblyQualifiedName);
                widget.WriteToOutput(output);
            }
        }
    }
}
