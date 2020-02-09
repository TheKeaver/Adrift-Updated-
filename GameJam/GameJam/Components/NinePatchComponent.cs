using System;
using Audrey;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Components
{
    public class NinePatchComponent : IComponent
    {
        public NinePatchComponent()
        {
        }

        public NinePatchComponent(NinePatchRegion2D ninePatch, Vector2 bounds, float thickness)
        {
            NinePatch = ninePatch;
            Bounds = bounds;
            Thickness = thickness;
        }

        public NinePatchRegion2D NinePatch;
        public Vector2 Bounds;
        public float Thickness;
        public Color Color = Color.White;
        public float Alpha = 1;

        public byte RenderGroup = 0x1;

        public bool Hidden;

        public float Depth = 0;
    }
}
