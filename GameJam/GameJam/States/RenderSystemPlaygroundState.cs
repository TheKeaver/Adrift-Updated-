using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.States
{
    public class RenderSystemPlaygroundState : CommonGameState
    {
        // RenderSystem already exists inside SharedGameState

        public RenderSystemPlaygroundState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
        }

        protected override void OnInitialize()
        {
            {
                Entity entity = SharedState.Engine.CreateEntity();
                entity.AddComponent(new TransformComponent(new Vector2(-20, 0)));
                entity.AddComponent(new SpriteComponent(new TextureRegion2D(Content.Load<Texture2D>("texture_shooter_enemy")),
                    new Vector2(100, 100)));
                entity.GetComponent<SpriteComponent>().Color = Color.Red;
                entity.GetComponent<SpriteComponent>().Depth = 100;
            }
            {
                Entity entity = SharedState.Engine.CreateEntity();
                entity.AddComponent(new TransformComponent());
                entity.AddComponent(new SpriteComponent(new TextureRegion2D(Content.Load<Texture2D>("texture_shooter_enemy")),
                    new Vector2(100, 100)));
                entity.GetComponent<SpriteComponent>().Color = Color.Green;
                entity.GetComponent<SpriteComponent>().Depth = 50;
            }
            {
                Entity entity = SharedState.Engine.CreateEntity();
                entity.AddComponent(new TransformComponent(new Vector2(20, 0)));
                entity.AddComponent(new SpriteComponent(new TextureRegion2D(Content.Load<Texture2D>("texture_shooter_enemy")),
                    new Vector2(100, 100)));
                entity.GetComponent<SpriteComponent>().Color = Color.Blue;
                entity.GetComponent<SpriteComponent>().Depth = 10;
            }

            {
                Entity entity = SharedState.Engine.CreateEntity();
                entity.AddComponent(new TransformComponent(new Vector2(-20, -150)));
                entity.AddComponent(new SpriteComponent(new TextureRegion2D(Content.Load<Texture2D>("texture_shooter_enemy")),
                    new Vector2(100, 100)));
                entity.GetComponent<SpriteComponent>().Color = Color.Red;
                entity.GetComponent<SpriteComponent>().Depth = 10;
            }
            {
                Entity entity = SharedState.Engine.CreateEntity();
                entity.AddComponent(new TransformComponent(new Vector2(0, -150)));
                entity.AddComponent(new SpriteComponent(new TextureRegion2D(Content.Load<Texture2D>("texture_shooter_enemy")),
                    new Vector2(100, 100)));
                entity.GetComponent<SpriteComponent>().Color = Color.Green;
                entity.GetComponent<SpriteComponent>().Depth = 50;
            }
            {
                Entity entity = SharedState.Engine.CreateEntity();
                entity.AddComponent(new TransformComponent(new Vector2(20, -150)));
                entity.AddComponent(new SpriteComponent(new TextureRegion2D(Content.Load<Texture2D>("texture_shooter_enemy")),
                    new Vector2(100, 100)));
                entity.GetComponent<SpriteComponent>().Color = Color.Blue;
                entity.GetComponent<SpriteComponent>().Depth = 100;
            }

            base.OnInitialize();
        }

        protected override void OnFixedUpdate(float dt)
        {
            base.OnFixedUpdate(dt);
        }
    }
}
