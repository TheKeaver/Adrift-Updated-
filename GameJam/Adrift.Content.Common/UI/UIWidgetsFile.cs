using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
#if PIPELINE
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
#endif

namespace Adrift.Content.Common.UI
{
    [Serializable]
    [XmlRoot("UI")]
    public class UIWidgetsFile
    {
        [XmlArray("Widgets")]
        [XmlArrayItem("Label", typeof(LabelWidgetPrototype)),
            XmlArrayItem("Image", typeof(ImageWidgetPrototype)),
            XmlArrayItem("NinePatchImage", typeof(NinePatchImageWidgetPrototype)),
            XmlArrayItem("Panel", typeof(PanelWidgetPrototype)),
            XmlArrayItem("Button", typeof(ButtonWidgetPrototype)),
            XmlArrayItem("DropDownPanel", typeof(DropDownPanelWidgetPrototype)),
            XmlArrayItem("External", typeof(ExternalWidgetPrototype)),
            XmlArrayItem("Slider", typeof(SliderWidgetPrototype))]
        public List<WidgetPrototype> Widgets
        {
            get;
            set;
        }
    }

    [Serializable]
    [XmlInclude(typeof(LabelWidgetPrototype))]
    public class WidgetPrototype
    {
        [XmlAttribute("halign")]
        public string Halign = "center";
        [XmlAttribute("valign")]
        public string Valign = "center";
        [XmlAttribute("horizontal")]
        public string Horizontal = "0";
        [XmlAttribute("vertical")]
        public string Vertical = "0";

        [XmlAttribute("width")]
        public string Width = "100%";
        [XmlAttribute("height")]
        public string Height = "100%";

        [XmlAttribute("hidden")]
        public bool Hidden = false;

        [XmlAttribute("alpha")]
        public float Alpha = 1;

        [XmlAttribute("aspect-ratio")]
        public string AspectRatio = "";

        [XmlAttribute("id")]
        public string ID = "";

        [XmlAttribute("class")]
        public string Class = "";

        [XmlElement("Label", typeof(LabelWidgetPrototype))]
        [XmlElement("Image", typeof(ImageWidgetPrototype))]
        [XmlElement("NinePatchImage", typeof(NinePatchImageWidgetPrototype))]
        [XmlElement("Panel", typeof(PanelWidgetPrototype))]
        [XmlElement("Button", typeof(ButtonWidgetPrototype))]
        [XmlElement("DropDownPanel", typeof(DropDownPanelWidgetPrototype))]
        [XmlElement("External", typeof(ExternalWidgetPrototype))]
        [XmlElement("Slider", typeof(SliderWidgetPrototype))]
        public List<WidgetPrototype> Children;

        /* Update self first, then base */
#if PIPELINE
        public virtual void WriteToOutput(ContentWriter output)
        {
            output.Write(Halign);
            output.Write(Valign);
            output.Write(Horizontal);
            output.Write(Vertical);
            output.Write(Width);
            output.Write(Height);
            output.Write(Hidden);
            output.Write(Alpha);
            output.Write(AspectRatio);
            output.Write(ID);
            output.Write(Class);

            output.Write(Children.Count);
            foreach (WidgetPrototype widget in Children)
            {
                output.Write(widget.GetType().AssemblyQualifiedName);
                widget.WriteToOutput(output);
            }
        }
#endif
        public virtual void ReadFromInput(ContentReader input)
        {
            Halign = input.ReadString();
            Valign = input.ReadString();
            Horizontal = input.ReadString();
            Vertical = input.ReadString();
            Width = input.ReadString();
            Height = input.ReadString();
            Hidden = input.ReadBoolean();
            Alpha = input.ReadSingle();
            AspectRatio = input.ReadString();
            ID = input.ReadString();
            Class = input.ReadString();

            int count = input.ReadInt32();
            Children = new List<WidgetPrototype>();
            for (int i = 0; i < count; i++)
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
                Children.Add(widget);
            }
        }
    }

    [Serializable]
    public class ExternalWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("src")]
        public string Source;

        /* Update self first, then base */
#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Source);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            Source = input.ReadString();

            base.ReadFromInput(input);
        }
    }

    [Serializable]
    public class LabelWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("content")]
        public string Content;
        [XmlAttribute("font")]
        public string Font;

#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Content.Replace("\\n", "\n"));
            output.Write(Font);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            Content = input.ReadString();
            Font = input.ReadString();

            base.ReadFromInput(input);
        }
    }

    [Serializable]
    public class ImageWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("image")]
        public string Image;

#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Image);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            Image = input.ReadString();

            base.ReadFromInput(input);
        }
    }

    [Serializable]
    public class NinePatchImageWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("image")]
        public string Image;

        [XmlAttribute("thickness")]
        public string Thickness;

#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Image);
            output.Write(Thickness);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            Image = input.ReadString();
            Thickness = input.ReadString();

            base.ReadFromInput(input);
        }
    }

    [Serializable]
    public class PanelWidgetPrototype : WidgetPrototype
    {
#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            base.ReadFromInput(input);
        }
    }


    [Serializable]
    public class ButtonWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("released-image")]
        public string ReleasedImage;
        [XmlAttribute("released-thickness")]
        public string ReleasedThickness;

        [XmlAttribute("hover-image")]
        public string HoverImage;
        [XmlAttribute("hover-thickness")]
        public string HoverThickness;

        [XmlAttribute("pressed-image")]
        public string PressedImage;
        [XmlAttribute("pressed-thickness")]
        public string PressedThickness;

        [XmlAttribute("onclick")]
        public string OnClick = "";

        [XmlAttribute("aboveID")]
        public string AboveID = "";
        [XmlAttribute("leftID")]
        public string LeftID = "";
        [XmlAttribute("rightID")]
        public string RightID = "";
        [XmlAttribute("belowID")]
        public string BelowID = "";

        [XmlAttribute("defaultSelected")]
        public bool IsSelected = false;

#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(ReleasedImage);
            output.Write(ReleasedThickness);
            output.Write(HoverImage);
            output.Write(HoverThickness);
            output.Write(PressedImage);
            output.Write(PressedThickness);
            output.Write(AboveID);
            output.Write(LeftID);
            output.Write(RightID);
            output.Write(BelowID);

            output.Write(IsSelected);

            output.Write(OnClick);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            ReleasedImage = input.ReadString();
            ReleasedThickness = input.ReadString();
            HoverImage = input.ReadString();
            HoverThickness = input.ReadString();
            PressedImage = input.ReadString();
            PressedThickness = input.ReadString();

            AboveID = input.ReadString();
            LeftID = input.ReadString();
            RightID = input.ReadString();
            BelowID = input.ReadString();

            IsSelected = input.ReadBoolean();

            OnClick = input.ReadString();

            base.ReadFromInput(input);
        }
    }

    [Serializable]
    public class SliderWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("released-image")]
        public string ReleasedImage;
        [XmlAttribute("released-thickness")]
        public string ReleasedThickness;

        [XmlAttribute("hover-image")]
        public string HoverImage;
        [XmlAttribute("hover-thickness")]
        public string HoverThickness;

        [XmlAttribute("pressed-image")]
        public string PressedImage;
        [XmlAttribute("pressed-thickness")]
        public string PressedThickness;

        [XmlAttribute("isVertical")]
        public bool isVertical;
        [XmlAttribute("isHorizontal")]
        public bool isHorizontal;

        [XmlAttribute("divisions")]
        public int divisions;

        [XmlAttribute("aboveID")]
        public string AboveID = "";
        [XmlAttribute("leftID")]
        public string LeftID = "";
        [XmlAttribute("rightID")]
        public string RightID = "";
        [XmlAttribute("belowID")]
        public string BelowID = "";

        [XmlAttribute("defaultSelected")]
        public bool IsSelected = false;

#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(ReleasedImage);
            output.Write(ReleasedThickness);
            output.Write(HoverImage);
            output.Write(HoverThickness);
            output.Write(PressedImage);
            output.Write(PressedThickness);
            output.Write(AboveID);
            output.Write(LeftID);
            output.Write(RightID);
            output.Write(BelowID);

            output.Write(IsSelected);

            output.Write(isHorizontal);
            output.Write(isVertical);

            output.Write(divisions);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            ReleasedImage = input.ReadString();
            ReleasedThickness = input.ReadString();
            HoverImage = input.ReadString();
            HoverThickness = input.ReadString();
            PressedImage = input.ReadString();
            PressedThickness = input.ReadString();

            AboveID = input.ReadString();
            LeftID = input.ReadString();
            RightID = input.ReadString();
            BelowID = input.ReadString();

            IsSelected = input.ReadBoolean();

            isHorizontal = input.ReadBoolean();
            isVertical = input.ReadBoolean();

            divisions = input.ReadInt32();

            base.ReadFromInput(input);
        }
    }

    [Serializable]
    public class DropDownPanelContentsInfo
    {
        [XmlAttribute("width")]
        public string Width;
        [XmlAttribute("height")]
        public string Height;

        [XmlElement("Label", typeof(LabelWidgetPrototype))]
        [XmlElement("Image", typeof(ImageWidgetPrototype))]
        [XmlElement("NinePatchImage", typeof(NinePatchImageWidgetPrototype))]
        [XmlElement("Panel", typeof(PanelWidgetPrototype))]
        [XmlElement("Button", typeof(ButtonWidgetPrototype))]
        [XmlElement("DropDownPanel", typeof(DropDownPanelWidgetPrototype))]
        [XmlElement("Slider", typeof(SliderWidgetPrototype))]
        public List<WidgetPrototype> Children;

#if PIPELINE
        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Width);
            output.Write(Height);

            output.Write(Children.Count);
            foreach (WidgetPrototype widget in Children)
            {
                output.Write(widget.GetType().AssemblyQualifiedName);
                widget.WriteToOutput(output);
            }
        }
#endif
        public void ReadFromInput(ContentReader input)
        {
            Width = input.ReadString();
            Height = input.ReadString();

            int count = input.ReadInt32();
            Children = new List<WidgetPrototype>();
            for (int i = 0; i < count; i++)
            {
                string assemblyName = GetType().Assembly.FullName.Split(',')[0];
                string className = input.ReadString().Split(',')[0];
                string assemblyQualifiedName = string.Format("{0}, {1}", className, assemblyName);
                WidgetPrototype widget = (WidgetPrototype)Activator.CreateInstance(Type.GetType(assemblyQualifiedName));
                widget.ReadFromInput(input);
                Children.Add(widget);
            }
        }
    }
    [Serializable]
    public class DropDownPanelWidgetPrototype : WidgetPrototype
    {
        [XmlAttribute("released-image")]
        public string ReleasedImage;
        [XmlAttribute("released-thickness")]
        public string ReleasedThickness;

        [XmlAttribute("hover-image")]
        public string HoverImage;
        [XmlAttribute("hover-thickness")]
        public string HoverThickness;

        [XmlAttribute("pressed-image")]
        public string PressedImage;
        [XmlAttribute("pressed-thickness")]
        public string PressedThickness;

        [XmlAttribute("close-on")]
        public string CloseOn;

        [XmlAttribute("aboveID")]
        public string AboveID = "";
        [XmlAttribute("leftID")]
        public string LeftID = "";
        [XmlAttribute("rightID")]
        public string RightID = "";
        [XmlAttribute("belowID")]
        public string BelowID = "";

        [XmlAttribute("defaultSelected")]
        public bool IsSelected = false;

        [XmlElement("Contents", IsNullable = false)]
        public DropDownPanelContentsInfo ContentsInfo;

#if PIPELINE
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(ReleasedImage);
            output.Write(ReleasedThickness);
            output.Write(HoverImage);
            output.Write(HoverThickness);
            output.Write(PressedImage);
            output.Write(PressedThickness);

            output.Write(CloseOn);

            output.Write(AboveID);
            output.Write(LeftID);
            output.Write(RightID);
            output.Write(BelowID);

            output.Write(IsSelected);

            ContentsInfo.WriteToOutput(output);

            base.WriteToOutput(output);
        }
#endif
        public override void ReadFromInput(ContentReader input)
        {
            ReleasedImage = input.ReadString();
            ReleasedThickness = input.ReadString();
            HoverImage = input.ReadString();
            HoverThickness = input.ReadString();
            PressedImage = input.ReadString();
            PressedThickness = input.ReadString();

            CloseOn = input.ReadString();

            AboveID = input.ReadString();
            LeftID = input.ReadString();
            RightID = input.ReadString();
            BelowID = input.ReadString();

            IsSelected = input.ReadBoolean();

            ContentsInfo = new DropDownPanelContentsInfo();
            ContentsInfo.ReadFromInput(input);

            base.ReadFromInput(input);
        }
    }

    public class UIWidgetsFileLoader
    {
        public static UIWidgetsFile Load(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(UIWidgetsFile));
            TextReader textReader = new StreamReader(filename);
            UIWidgetsFile file = (UIWidgetsFile)deserializer.Deserialize(textReader);
            textReader.Close();
            return file;
        }
    }
}
