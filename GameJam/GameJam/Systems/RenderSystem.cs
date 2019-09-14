using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using GameJam.Components;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Sprites;

namespace GameJam.Systems
{
    public class RenderSystem
    {
        protected Engine Engine
        {
            get;
            private set;
        }

        public static readonly Vector2 FlipY = new Vector2(1, -1);
        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

        readonly Family _spriteFamily = Family.All(typeof(SpriteComponent), typeof(TransformComponent)).Get();
        readonly Family _fontFamily = Family.All(typeof(FontComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _spriteEntities;
        readonly ImmutableList<Entity> _fontEntities;

        public SpriteBatch SpriteBatch { get; }

        public RenderSystem(GraphicsDevice graphics, Engine engine)
        {
            Engine = engine;
            _spriteEntities = Engine.GetEntitiesFor(_spriteFamily);
            _fontEntities = Engine.GetEntitiesFor(_fontFamily);

            SpriteBatch = new SpriteBatch(graphics);
        }

        public void DrawEntities(float dt, float betweenFrameAlpha)
        {
            DrawEntities(Constants.Render.GROUP_MASK_ALL, dt, betweenFrameAlpha);
        }

        public void DrawEntities(byte groupMask, float dt, float betweenFrameAlpha)
        {
            DrawEntities(Matrix.Identity, groupMask, dt, betweenFrameAlpha);
        }

        public void DrawEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            DrawSpriteBatchEntities(transformMatrix, groupMask, dt, betweenFrameAlpha);
            DrawVectorEntities(transformMatrix, groupMask, dt, betweenFrameAlpha);
        }

        private void DrawSpriteBatchEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                               null,
                               SamplerState.PointClamp,
                               null,
                               null,
                               null,
                               transformMatrix);

            foreach (Entity entity in _spriteEntities)
            {
                SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
                if (spriteComp.Hidden
                    || (spriteComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 offsetPosition = (transformComp.Position - transformComp.LastPosition) * (1 - betweenFrameAlpha);
                offsetPosition *= -1;

                Vector2 scale = new Vector2(spriteComp.Bounds.X / spriteComp.Texture.Width,
                                            spriteComp.Bounds.Y / spriteComp.Texture.Height);
				float transformScale = transformComp.Scale + (transformComp.Scale - transformComp.LastScale) * (1 - betweenFrameAlpha);
                Vector2 origin = new Vector2(spriteComp.Texture.Bounds.Width,
                                             spriteComp.Texture.Bounds.Height) * HalfHalf;

                AnimationComponent animationComp = entity.GetComponent<AnimationComponent>();
                if (animationComp != null && animationComp.ActiveAnimationIndex > -1)
                {
                    Rectangle sourceRectangle = animationComp.Animations[animationComp.ActiveAnimationIndex].TextureRegion.Bounds;
                    scale = new Vector2(spriteComp.Bounds.X / sourceRectangle.Width,
                                            spriteComp.Bounds.Y / sourceRectangle.Height);
                    origin = new Vector2(sourceRectangle.Width,
                                             sourceRectangle.Height) * HalfHalf;

                    SpriteBatch.Draw(animationComp.Animations[animationComp.ActiveAnimationIndex].TextureRegion.Texture,
                                      (transformComp.Position + offsetPosition) * FlipY,
                                      sourceRectangle,
                                      Color.White,
                                      -transformComp.Rotation,
                                      origin,
                                      scale * transformScale,
                                      SpriteEffects.None,
                                      0);
                }
                else
                {
                    SpriteBatch.Draw(spriteComp.Texture,
                                      (transformComp.Position + offsetPosition) * FlipY,
                                      null,
                                      Color.White,
                                      -transformComp.Rotation,
                                      origin,
                                      scale,
                                      SpriteEffects.None,
                                      0);
                }
            }

            foreach (Entity entity in _fontEntities)
            {
                FontComponent fontComp = entity.GetComponent<FontComponent>();
                if (fontComp.Hidden
                    || (fontComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 scale = Vector2.One;
				float transformScale = transformComp.Scale + (transformComp.Scale - transformComp.LastScale) * (1 - betweenFrameAlpha);
				Vector2 origin = fontComp.Font.MeasureString(fontComp.Content) / 2;

                SpriteBatch.DrawString(fontComp.Font,
                                        fontComp.Content,
                                        transformComp.Position * FlipY,
                                        fontComp.Color,
                                        -transformComp.Rotation,
                                        origin,
                                        scale * transformScale,
                                        SpriteEffects.None,
                                        0);
            }

            SpriteBatch.End();
        }
    }
}
