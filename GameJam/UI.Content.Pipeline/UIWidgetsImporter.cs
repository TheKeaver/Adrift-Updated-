﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace UI.Content.Pipeline
{
    [ContentImporter(".xml", DefaultProcessor = "UIWidgetsProcessor",
        DisplayName = "UI Widgets - Adrift")]
    public class UIWidgetsImporter : ContentImporter<UIWidgetsFile>
    {
        public override UIWidgetsFile Import(string filename, ContentImporterContext context)
        {
            using (StreamReader streamReader = new StreamReader(filename))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(UIWidgetsFile));
                return (UIWidgetsFile)deserializer.Deserialize(streamReader);
            }
        }
    }
}
