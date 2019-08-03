using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Components
{
    /// <summary>
    /// A component for holding a renderable sprite.
    /// </summary>
    public class SpriteComponent : IComponent
    {
        public SpriteComponent()
        {
        }

        public SpriteComponent(Texture2D texture, Vector2 bounds)
        {
            Texture = texture;
            Bounds = bounds;
        }

        public Texture2D Texture;
        public Vector2 Bounds;

        public byte RenderGroup = 0x1;

        public bool Hidden;
    }
}
