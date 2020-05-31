using System;
using Audrey;
using FontExtension;
using GameJam.Common;
using GameJam.Components;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    public class MSDFFontTestGameState : GameState
    {
        private Engine Engine;
        private FieldFontRenderer _textRenderer;
        private float _elapsed;

        readonly Family fontFamily = Family.All(typeof(TransformComponent), typeof(FieldFontComponent)).Get();

        private Camera Camera;

        public MSDFFontTestGameState(GameManager gameManager) : base(gameManager)
        {
        }

        protected override void OnInitialize()
        {
            _textRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);

            Engine = new Engine();
            Entity entity = Engine.CreateEntity();
            entity.AddComponent(new FieldFontComponent(Content.Load<FieldFont>("font_msdf_hyperspace"), "The quick brown fox jumped over the lazy dog!?"));
            entity.GetComponent<FieldFontComponent>().OptimizeForSmallText = true;
            entity.AddComponent(new TransformComponent());
            entity.GetComponent<TransformComponent>().SetPosition(new Vector2(0, 0), true);
            entity.GetComponent<TransformComponent>().SetScale(1, true);

            Camera = new Camera(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);

            base.OnInitialize();
        }

        protected override void OnFixedUpdate(float dt)
        {
            _elapsed += dt;

            foreach (Entity entity in Engine.GetEntitiesFor(fontFamily))
            {
                entity.GetComponent<TransformComponent>().Move(new Vector2(200 * (float)Math.Cos(_elapsed), 200 * (float)Math.Sin(_elapsed)) - entity.GetComponent<TransformComponent>().Position);
                entity.GetComponent<TransformComponent>().Rotate(-MathHelper.Pi / 4 * dt);
            }

            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            //var world = Matrix.Identity;
            //var view = Matrix.CreateLookAt(Vector3.Backward, Vector3.Forward, Vector3.Up);
            //var projection = Matrix.CreatePerspectiveFieldOfView(
            //    MathHelper.PiOver2,
            //    GameManager.GraphicsDevice.Viewport.Width / (float)GameManager.GraphicsDevice.Viewport.Height,
            //    0.01f,
            //    1000.0f);

            //var wvp = world * view * projection;

            /** Based on MonoGame SpriteBatch implementation!! **/

            // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
            // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
            // --> We get the correct matrix with near plane 0 and far plane -1.
            Matrix projection;
            //Matrix.CreateOrthographicOffCenter(0, GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height,
            //    0, 0, -1, out projection);
            Matrix.CreateOrthographicOffCenter(0, GameManager.GraphicsDevice.Viewport.Width, 0,
                GameManager.GraphicsDevice.Viewport.Height, -1, 1, out projection);

            Matrix wvp = Camera.TransformMatrix * projection;

            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            _textRenderer.Begin(wvp);
            foreach (Entity entity in Engine.GetEntitiesFor(fontFamily))
            {
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                FieldFontComponent fieldFontComp = entity.GetComponent<FieldFontComponent>();

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float rotation = transformComp.Rotation
                    + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;
                rotation *= -1;

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                _textRenderer.Draw(fieldFontComp.Font, fieldFontComp.Content,
                    position, rotation, fieldFontComp.Color, transformScale,
                    fieldFontComp.EnableKerning);
            }
            _textRenderer.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void RegisterListeners()
        {
            Camera.RegisterEvents();

            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            Camera.UnregisterEvents();

            base.UnregisterListeners();
        }
    }
}
