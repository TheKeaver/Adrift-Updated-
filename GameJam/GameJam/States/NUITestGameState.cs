﻿using FontExtension;
using GameJam.NUI;
using GameJam.NUI.Widgets;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.States
{
    public class NUITestGameState : CommonGameState
    {
        public Root Root
        {
            get;
            private set;
        }

        public NUITestGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
        }

        protected override void OnInitialize()
        {
            Root = new Root(SharedState.Engine);

            {
                ContainerWidget container = new ContainerWidget(SharedState.Engine);
                container.HAlign = new FixedValue<HorizontalAlignment>(HorizontalAlignment.Left);
                container.Width = new RelativeValue<float>(Root, "width", 0.5f);
                container.Height = new RelativeValue<float>(Root, "height", 1.0f);
                Root.Add(container);

                ImageWidget image = new ImageWidget(SharedState.Engine);
                image.Image = new FixedValue<TextureRegion2D>(Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_input_keyboard_key_0"));
                image.Width = new FixedValue<float>(100);
                image.Height = new FixedValue<float>(100);

                container.Add(image);

                TextWidget text = new TextWidget(SharedState.Engine);
                text.Font = new FixedValue<FieldFont>(Content.Load<FieldFont>("font_msdf_hyperspace"));
                text.Content = new FixedValue<string>("Hello World!");
                text.Height = new FixedValue<float>(100);
                //text.VAlign = new FixedValue<VerticalAlignment>(VerticalAlignment.Top);

                Root.Add(text);
            }

            base.OnInitialize();
        }
    }
}
