using FontExtension;
using GameJam.NUI;
using GameJam.NUI.Widgets;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using static GameJam.NUI.Widgets.TableWidget;
using static GameJam.NUI.Widgets.TableWidget.RowWidget;

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
                RowWidget row = new RowWidget(SharedState.Engine);
                row.Units = new FixedValue<int>(6);
                Root.Add(row);
                ColumnWidget column = new ColumnWidget(SharedState.Engine);
                row.Add(column);

                column.Padding = new FixedValue<float>(50.0f);

                //ContainerWidget container = new ContainerWidget(SharedState.Engine);
                //container.HAlign = new FixedValue<HorizontalAlignment>(HorizontalAlignment.Left);
                //container.Width = new RelativeValue<float>(Root, "width", 0.5f);
                //container.Height = new RelativeValue<float>(Root, "height", 1.0f);
                //container.Padding = new FixedValue<float>(100.0f);
                //Root.Add(container);

                NinePatchImageWidget ninePatch = new NinePatchImageWidget(SharedState.Engine);
                ninePatch.Image = new FixedValue<NinePatchRegion2D>(new NinePatchRegion2D(Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_ui_button_pressed"), 5));
                ninePatch.Width = new RelativeValue<float>(column, "inner-width", 1.0f);
                ninePatch.Height = new RelativeValue<float>(column, "inner-height", 1.0f);
                column.Add(ninePatch);

                //ImageWidget image = new ImageWidget(SharedState.Engine);
                //image.Image = new FixedValue<TextureRegion2D>(new TextureRegion2D(Content.Load<Texture2D>("texture_background_parallax_test")));
                //image.Width = new RelativeValue<float>(container, "inner-width", 1.0f);
                //image.Height = new RelativeValue<float>(container, "inner-height", 1.0f);
                //container.Add(image);

                //TextWidget text = new TextWidget(SharedState.Engine);
                //text.Font = new FixedValue<FieldFont>(Content.Load<FieldFont>("font_msdf_hyperspace"));
                //text.Content = new FixedValue<string>("Hello World!\nThis is another line!");
                //text.Height = new FixedValue<float>(100);
                //text.Justify = new FixedValue<FieldFontJustify>(FieldFontJustify.Left);
                //text.VAlign = new FixedValue<VerticalAlignment>(VerticalAlignment.Center);
                //text.HAlign = new FixedValue<HorizontalAlignment>(HorizontalAlignment.Center);
                //Root.Add(text);
            }
            {
                RowWidget row = new RowWidget(SharedState.Engine);
                row.Units = new FixedValue<int>(2);
                row.VAlign = new FixedValue<VerticalAlignment>(VerticalAlignment.Bottom);
                Root.Add(row);
                {
                    ColumnWidget column = new ColumnWidget(SharedState.Engine);
                    row.Add(column);

                    column.Padding = new FixedValue<float>(10);
                    column.Units = new FixedValue<int>(4);

                    NinePatchImageWidget ninePatch = new NinePatchImageWidget(SharedState.Engine);
                    ninePatch.Image = new FixedValue<NinePatchRegion2D>(new NinePatchRegion2D(Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_ui_button_pressed"), 5));
                    ninePatch.Width = new RelativeValue<float>(column, "inner-width", 1.0f);
                    ninePatch.Height = new RelativeValue<float>(column, "inner-height", 1.0f);
                    column.Add(ninePatch);
                }
                {
                    ColumnWidget column = new ColumnWidget(SharedState.Engine);
                    row.Add(column);

                    column.Padding = new FixedValue<float>(10);
                    column.Units = new FixedValue<int>(4);

                    NinePatchImageWidget ninePatch = new NinePatchImageWidget(SharedState.Engine);
                    ninePatch.Image = new FixedValue<NinePatchRegion2D>(new NinePatchRegion2D(Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_ui_button_pressed"), 5));
                    ninePatch.Width = new RelativeValue<float>(column, "inner-width", 1.0f);
                    ninePatch.Height = new RelativeValue<float>(column, "inner-height", 1.0f);
                    column.Add(ninePatch);
                }
                {
                    ColumnWidget column = new ColumnWidget(SharedState.Engine);
                    row.Add(column);

                    column.Padding = new FixedValue<float>(10);
                    column.Units = new FixedValue<int>(4);

                    NinePatchImageWidget ninePatch = new NinePatchImageWidget(SharedState.Engine);
                    ninePatch.Image = new FixedValue<NinePatchRegion2D>(new NinePatchRegion2D(Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_ui_button_pressed"), 5));
                    ninePatch.Width = new RelativeValue<float>(column, "inner-width", 1.0f);
                    ninePatch.Height = new RelativeValue<float>(column, "inner-height", 1.0f);
                    column.Add(ninePatch);
                }
            }
            //{
            //    RowWidget row = new RowWidget(SharedState.Engine);
            //    row.Units = new FixedValue<int>(6);
            //    Root.Add(row);
            //    ColumnWidget column = new ColumnWidget(SharedState.Engine);
            //    row.Add(column);

            //    column.Padding = new FixedValue<float>(100.0f);

            //    NinePatchImageWidget ninePatch = new NinePatchImageWidget(SharedState.Engine);
            //    ninePatch.Image = new FixedValue<NinePatchRegion2D>(new NinePatchRegion2D(Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_ui_button_pressed"), 5));
            //    ninePatch.Width = new RelativeValue<float>(column, "inner-width", 1.0f);
            //    ninePatch.Height = new RelativeValue<float>(column, "inner-height", 1.0f);
            //    column.Add(ninePatch);
            //}

            /*
             * <Row units="6">
             *  <Column padding="100">
             *      <NinePatchImage image="complete_texture_atlas" width="100%" height="100%" />
             *  </Column>
             * </Row>
             * <Row units="6">
             *  <Column padding="100">
             *      <NinePatchImage image="complete_texture_atlas" width="100%" height="100%" />
             *  </Column>
             * </Row>
             */

            base.OnInitialize();
        }
    }
}
