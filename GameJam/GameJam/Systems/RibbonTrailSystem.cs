using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Systems
{
    public class RibbonTrailSystem : BaseSystem
    {
        private readonly Family _ribbonFamily = Family.All(typeof(RibbonTrailComponent), typeof(TransformHistoryComponent)).Get();

        private readonly ImmutableList<Entity> _ribbonEntities;

        private readonly Vector2 FlipY = new Vector2(1, -1);

        public RibbonTrailSystem(Engine engine) : base(engine)
        {
            _ribbonEntities = Engine.GetEntitiesFor(_ribbonFamily);
        }

        public override void Update(float dt)
        {
            foreach(Entity entity in _ribbonEntities)
            {
                TransformHistoryComponent transformHistoryComp = entity.GetComponent<TransformHistoryComponent>();
                RibbonTrailComponent ribbonTrailComp = entity.GetComponent<RibbonTrailComponent>();

                if(transformHistoryComp.positionHistory.Length != transformHistoryComp.rotationHistory.Length)
                {
                    throw new Exception("Position and rotation history lengths do not match.");
                }

                int len = transformHistoryComp.positionHistory.Length;
                for (int i = 0; i < len; i++)
                {
                    int v1 = i * 2;
                    int v2 = i * 2 + 1;
                    int nv1 = (i * 2 + 2) % (2 * len);
                    int nv2 = (i * 2 + 3) % (2 * len);

                    int i1 = i * 6;
                    int i2 = i * 6 + 1;
                    int i3 = i * 6 + 2;
                    int i4 = i * 6 + 3;
                    int i5 = i * 6 + 4;
                    int i6 = i * 6 + 5;

                    float alpha = MathHelper.Lerp(1, 0, (float)i / len);

                    Vector2 pos = transformHistoryComp.GetTransformHistoryAt(-(i + 1)) * FlipY;
                    Vector2 p1, p2;
                    Vector2 direction = pos - transformHistoryComp.GetTransformHistoryAt(-(i + 1) - 1) * FlipY;
                    direction.Normalize();
                    CalculateEndsAtLocation(Vector2.Zero, pos, direction,
                        out p1, out p2);
                    ribbonTrailComp.Verts[v1].Position.X = p1.X;
                    ribbonTrailComp.Verts[v1].Position.Y = p1.Y;
                    // TODO: Change to shader?
                    ribbonTrailComp.Verts[v1].Color.R = ribbonTrailComp.Color.R;
                    ribbonTrailComp.Verts[v1].Color.G = ribbonTrailComp.Color.G;
                    ribbonTrailComp.Verts[v1].Color.B = ribbonTrailComp.Color.B;
                    ribbonTrailComp.Verts[v1].Color.A = (byte)(MathHelper.Lerp(255, 0, alpha));
                    ribbonTrailComp.Verts[v2].Position.X = p2.X;
                    ribbonTrailComp.Verts[v2].Position.Y = p2.Y;
                    ribbonTrailComp.Verts[v2].Color.R = ribbonTrailComp.Color.R;
                    ribbonTrailComp.Verts[v2].Color.G = ribbonTrailComp.Color.G;
                    ribbonTrailComp.Verts[v2].Color.B = ribbonTrailComp.Color.B;
                    ribbonTrailComp.Verts[v2].Color.A = (byte)(MathHelper.Lerp(255, 0, alpha));

                    if (i < len - 1)
                    {
                        ribbonTrailComp.Indices[i1] = v1;
                        ribbonTrailComp.Indices[i2] = nv1;
                        ribbonTrailComp.Indices[i3] = v2;

                        ribbonTrailComp.Indices[i4] = v2;
                        ribbonTrailComp.Indices[i5] = nv1;
                        ribbonTrailComp.Indices[i6] = nv2;
                    }
                }
            }
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
    }
}
