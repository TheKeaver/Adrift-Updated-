using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace UI.Content.Pipeline
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
                WidgetPrototype widget = (WidgetPrototype)Activator.CreateInstance(Type.GetType(input.ReadString()));
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
        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Source);

            base.WriteToOutput(output);
        }
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

        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Content);
            output.Write(Font);

            base.WriteToOutput(output);
        }
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

        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Image);

            base.WriteToOutput(output);
        }
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

        public override void WriteToOutput(ContentWriter output)
        {
            output.Write(Image);
            output.Write(Thickness);

            base.WriteToOutput(output);
        }
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
        public override void WriteToOutput(ContentWriter output)
        {
            base.WriteToOutput(output);
        }
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

        //[XmlAttribute("onclick")]
        //public string OnClick = "";

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

            //output.Write(OnClick);

            base.WriteToOutput(output);
        }
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

            //OnClick = input.ReadString();

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
        public void ReadFromInput(ContentReader input)
        {
            Width = input.ReadString();
            Height = input.ReadString();

            int count = input.ReadInt32();
            Children = new List<WidgetPrototype>();
            for (int i = 0; i < count; i++)
            {
                WidgetPrototype widget = (WidgetPrototype)Activator.CreateInstance(Type.GetType(input.ReadString()));
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
