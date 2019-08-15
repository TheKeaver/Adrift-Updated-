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

        Family _spriteFamily = Family.All(typeof(SpriteComponent), typeof(TransformComponent)).Get();
        Family _fontFamily = Family.All(typeof(FontComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _spriteEntities;
        ImmutableList<Entity> _fontEntities;

        SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get
            {
                return _spriteBatch;
            }
        }

        public RenderSystem(GraphicsDevice graphics, Engine engine)
        {
            Engine = engine;
            _spriteEntities = Engine.GetEntitiesFor(_spriteFamily);
            _fontEntities = Engine.GetEntitiesFor(_fontFamily);

            _spriteBatch = new SpriteBatch(graphics);
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
            _spriteBatch.Begin(SpriteSortMode.Deferred,
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

                    _spriteBatch.Draw(animationComp.Animations[animationComp.ActiveAnimationIndex].TextureRegion.Texture,
                                      (transformComp.Position + offsetPosition) * FlipY,
                                      sourceRectangle,
                                      Color.White,
                                      -transformComp.Rotation,
                                      origin,
                                      scale,
                                      SpriteEffects.None,
                                      0);
                }
                else
                {
                    _spriteBatch.Draw(spriteComp.Texture,
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
                Vector2 origin = fontComp.Font.MeasureString(fontComp.Content) / 2;

                _spriteBatch.DrawString(fontComp.Font,
                                        fontComp.Content,
                                        transformComp.Position * FlipY,
                                        fontComp.Color,
                                        -transformComp.Rotation,
                                        origin,
                                        scale,
                                        SpriteEffects.None,
                                        0);
            }

            _spriteBatch.End();
        }
    }
}
