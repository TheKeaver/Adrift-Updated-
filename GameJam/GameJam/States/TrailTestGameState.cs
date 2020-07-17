using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameJam.States
{
    public class TrailTestGameState : CommonGameState
    {
        private BasicEffect _basicEffect;

        private Matrix _projection;

        private readonly int trailParts = 10;

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        private float _elapsed;
        private List<Vector2> _positions = new List<Vector2>();
        private List<float> _rotations = new List<float>();

        private VertexPositionColor[] _verts;
        private int[] _idxs;

        public TrailTestGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
        }

        protected override void OnInitialize()
        {
            _basicEffect = new BasicEffect(GameManager.GraphicsDevice);
            _basicEffect.TextureEnabled = false;
            _basicEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _basicEffect.VertexColorEnabled = true;

            _vertexBuffer = new VertexBuffer(GameManager.GraphicsDevice,
                typeof(VertexPositionColor),
                trailParts * 2 + 2,
                BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(GameManager.GraphicsDevice,
                IndexElementSize.ThirtyTwoBits,
                6 * trailParts,
                BufferUsage.WriteOnly);

            Viewport viewport = GameManager.GraphicsDevice.Viewport;

            // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
            // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
            // --> We get the correct matrix with near plane 0 and far plane -1.
            Matrix.CreateOrthographicOffCenter(-viewport.Width / 2, viewport.Width / 2, -viewport.Height / 2,
                viewport.Height / 2, -1, 1, out _projection);
            _basicEffect.World = _projection;

            _verts = new VertexPositionColor[trailParts * 2 + 2];
            for(int i = 0; i < _verts.Length; i++)
            {
                _verts[i] = new VertexPositionColor();
            }

            _idxs = new int[6 * trailParts];

            UploadVertexAndIndexData();

            base.OnInitialize();
        }

        protected override void OnFixedUpdate(float dt)
        {
            _elapsed += dt;

            Vector2 position = new Vector2(300 * (float)Math.Cos(_elapsed), 300 * (float)Math.Sin(_elapsed));
            _positions.Insert(0, position);
            _rotations.Insert(0, (float)Math.Atan2(position.Y, position.X) + MathHelper.PiOver2);
            if(_positions.Count > trailParts)
            {
                _positions.RemoveRange(trailParts, _positions.Count - trailParts);
            }
            if (_rotations.Count > trailParts)
            {
                _rotations.RemoveRange(trailParts, _rotations.Count - trailParts);
            }

            //////////////
            if(_positions.Count == trailParts)
            {
                Console.WriteLine("H");

                for(int i = 0; i < trailParts; i++)
                {
                    if(i == trailParts - 1)
                    {
                        continue;
                    }

                    int v1 = i * 2;
                    int v2 = i * 2 + 1;
                    int nv1 = (i * 2 + 2) % (2 * trailParts);
                    int nv2 = (i * 2 + 3) % (2 * trailParts);

                    int i1 = i * 6;
                    int i2 = i * 6 + 1;
                    int i3 = i * 6 + 2;
                    int i4 = i * 6 + 3;
                    int i5 = i * 6 + 4;
                    int i6 = i * 6 + 5;

                    float alpha = MathHelper.Lerp(1, 0, (float)i / trailParts);

                    Vector2 pos = _positions[i];
                    Vector2 p1, p2;
                    CalculateEndsAtLocation(Vector2.Zero, pos, new Vector2((float)Math.Cos(_rotations[i]), (float)Math.Sin(_rotations[i])),
                        out p1, out p2);
                    _verts[v1].Position.X = p1.X;
                    _verts[v1].Position.Y = p1.Y;
                    _verts[v1].Color.R = _verts[v1].Color.G = _verts[v1].Color.B = _verts[v1].Color.A = (byte)(255.0f * alpha);
                    _verts[v2].Position.X = p2.X;
                    _verts[v2].Position.Y = p2.Y;
                    _verts[v2].Color.R = _verts[v2].Color.G = _verts[v2].Color.B = _verts[v1].Color.A = (byte)(255.0f * alpha);

                    _idxs[i1] = v1;
                    _idxs[i2] = v2;
                    _idxs[i3] = nv1;

                    _idxs[i4] = v2;
                    _idxs[i6] = nv2;
                    _idxs[i5] = nv1;
                }

                UploadVertexAndIndexData();
            }
            //////////////

            base.OnFixedUpdate(dt);
        }

        private void CalculateEndsAtLocation(Vector2 origin, Vector2 location, Vector2 direction,
            out Vector2 p1, out Vector2 p2)
        {
            Vector2 normal = new Vector2(-direction.Y, direction.X);

            Vector2 lp1 = normal * CVars.Get<float>("animation_trail_width") / 2 + location;
            Vector2 lp2 = -normal * CVars.Get<float>("animation_trail_width") / 2 + location;

            p1 = lp1 - origin;
            p2 = lp2 - origin;
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            GameManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GameManager.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            GameManager.GraphicsDevice.Indices = _indexBuffer;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                    0,
                    0,
                    trailParts * 2);
            }

            base.OnRender(dt, betweenFrameAlpha);
        }

        private void UploadVertexAndIndexData()
        {
            _vertexBuffer.SetData(_verts);
            _indexBuffer.SetData(_idxs);
        }
    }
}
