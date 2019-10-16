using System;
using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Systems;
using Microsoft.Xna.Framework;

namespace GameJam.States
{
    public class TestRenderGameState : GameState
    {
        private Engine Engine;

        private RenderSystem _renderSystem;

        private Camera _camera;

        public TestRenderGameState(GameManager gameManager) : base(gameManager)
        {
        }

        public override void Dispose()
        {
            _camera.UnregisterEvents();
        }

        public override void Draw(float dt)
        {
            _renderSystem.DrawEntities(_camera.TransformMatrix, 0xFF, dt, 0);
        }

        public override void Hide()
        {
            
        }

        public override void Initialize()
        {
            Engine = new Engine();

            _renderSystem = new RenderSystem(GameManager.GraphicsDevice, Engine);

            _camera = new Camera(CVars.Get<int>("window_width"), CVars.Get<int>("window_height"));
            _camera.RegisterEvents();

            ////
            Entity entity = Engine.CreateEntity();
            entity.AddComponent(new TransformComponent());
            entity.AddComponent(new VectorSpriteComponent(new RenderShape[]{
                new PolyRenderShape(new Vector2[] {
                    new Vector2(-10, -100),
                    new Vector2(10, 100)
                }, 10, Color.Red, PolyRenderShape.PolyCapStyle.None, false)
            }));
        }

        public override void LoadContent()
        {
            
        }

        public override void Show()
        {

        }

        public override void Update(float dt)
        {
            Engine.GetEntities()[0].GetComponent<TransformComponent>().Rotate(10 * 3.141592f / 180 * dt);
        }
    }
}
