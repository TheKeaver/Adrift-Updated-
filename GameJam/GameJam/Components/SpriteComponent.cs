using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Components
{
    /// <summary>
    /// A component for holding a renderable sprite.
    /// </summary>
    public class SpriteComponent : IComponent, IRenderComponent
    {
        public SpriteComponent()
        {
        }

        public SpriteComponent(TextureRegion2D texture, Vector2 bounds)
        {
            Texture = texture;
            Bounds = bounds;
        }

        public BoundingRect GetAABB(float scale)
        {
            return new BoundingRect(-Bounds.X/2, -Bounds.Y/2, Bounds.X, Bounds.Y) * 2;
        }

        public bool IsHidden()
        {
            return Hidden;
        }

        public TextureRegion2D Texture;
        public Vector2 Bounds;
        public Color Color = Color.White;
        public float Alpha = 1;

        public byte RenderGroup = 0x1;

        public bool Hidden;

        public float Depth = 0;

        public byte GetRenderGroup()
        {
            return RenderGroup;
        }
    }
}
