using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameJam.Systems
{
    public class QuadTreeDebugRenderSystem
    {
        protected Engine Engine
        {
            get;
            private set;
        }

        public static readonly Vector2 FlipY = new Vector2(1, -1);
        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

        readonly Family _quadTreeFamily = Family.All(typeof(QuadTreeReferenceComponent)).Get();
        readonly ImmutableList<Entity> _quadTreeEntities;

        public SpriteBatch SpriteBatch { get;  }
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        public QuadTreeDebugRenderSystem(GraphicsDevice graphics, Engine engine)
        {
            Engine = engine;
            _quadTreeEntities = Engine.GetEntitiesFor(_quadTreeFamily);

            SpriteBatch = new SpriteBatch(graphics);
            GraphicsDevice = graphics;
        }

        public void Draw(Camera camera, float dt, Camera debugCamera = null)
        {
            Camera drawCamera = camera;
            if (debugCamera != null)
            {
                drawCamera = debugCamera;
            }

            Matrix transformMatrix = drawCamera.TransformMatrix;
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                    BlendState.Opaque,
                    SamplerState.AnisotropicClamp,
                    null,
                    null,
                    null,
                    transformMatrix);
            
            List<QuadTreeNode> drawnNodes = new List<QuadTreeNode>();

            foreach (Entity entity in _quadTreeEntities)
            {
                RecursiveDrawHelper(entity.GetComponent<QuadTreeReferenceComponent>().node, drawnNodes);
            }
            SpriteBatch.End();
        }

        private void RecursiveDrawHelper(QuadTreeNode node,  List<QuadTreeNode> drawnNodes)
        {
            if (node == null || drawnNodes.Contains(node))
                return;

            SpriteBatch.DrawRectangle(new Rectangle((int)node.boundingRect.Left,
                                                    (int)node.boundingRect.Bottom,
                                                    (int)node.boundingRect.Width,
                                                    (int)node.boundingRect.Height),
                                                    Color.DarkSlateGray);
            drawnNodes.Add(node);
            RecursiveDrawHelper(node.parent, drawnNodes);
        }
    }
}
