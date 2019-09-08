using System;
using GameJam.UINew;
using GameJam.UINew.Widgets;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using UI.Content.Pipeline;

namespace GameJam.UINew
{
    public static class WidgetFactory
    {
        public static Widget CreateFromPrototype(ContentManager content, WidgetPrototype prototype)
        {
            Widget widget = null;

            AbstractValue width = ValueFromString(prototype.Width);
            AbstractValue height = ValueFromString(prototype.Height);

            AbstractValue horizontal = ValueFromString(prototype.Horizontal);
            AbstractValue vertical = ValueFromString(prototype.Vertical);

            HorizontalAlignment halign;
            switch (prototype.Halign.ToLower()[0])
            {
                case 'r':
                    halign = HorizontalAlignment.Right;
                    break;
                case 'l':
                    halign = HorizontalAlignment.Left;
                    break;
                case 'c':
                default:
                    halign = HorizontalAlignment.Center;
                    break;
            }
            VerticalAlignment valign;
            switch (prototype.Valign.ToLower()[0])
            {
                case 't':
                    valign = VerticalAlignment.Top;
                    break;
                case 'b':
                    valign = VerticalAlignment.Bottom;
                    break;
                case 'c':
                default:
                    valign = VerticalAlignment.Center;
                    break;
            }

            /** WIDGET SPECIALIZATIONS **/
            if (prototype is LabelWidgetPrototype)
            {
                BitmapFont font = content.Load<BitmapFont>(CVars.Get<string>(((LabelWidgetPrototype)prototype).Font));
                string labelContent = ((LabelWidgetPrototype)prototype).Content;

                widget = new Label(font, labelContent, halign, horizontal, valign, vertical, width, height);
            }
            if(prototype is ImageWidgetPrototype)
            {
                Texture2D texture = content.Load<Texture2D>(CVars.Get<string>(((ImageWidgetPrototype)prototype).Image));

                widget = new Image(texture, halign, horizontal, valign, vertical, width, height);
            }
            if(prototype is PanelWidgetPrototype)
            {
                widget = new Panel(halign, horizontal, valign, vertical, width, height);
            }

            if(widget is IParentWidget)
            {
                foreach (WidgetPrototype childPrototype in prototype.Children)
                {
                    ((IParentWidget)widget).Add(CreateFromPrototype(content, childPrototype));
                }
            }

            widget.Hidden = prototype.Hidden;

            return widget;
        }

        private static AbstractValue ValueFromString(string s)
        {
            s = s.Trim();
            if(s[s.Length - 1] == '%')
            {
                // TODO: Percentage abstract value
                return null;
            }
            return new FixedValue(float.Parse(s));
        }
    }
}
