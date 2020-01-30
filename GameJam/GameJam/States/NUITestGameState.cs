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
                ImageWidget image = new ImageWidget(SharedState.Engine);
                image.Texture = Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_input_keyboard_key_0");
                image.Width = new FixedValue(100);
                image.Height = new FixedValue(100);

                Root.Add(image);
            }

            base.OnInitialize();
        }
    }
}
