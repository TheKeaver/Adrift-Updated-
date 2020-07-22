using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class RibbonTrailMovementThresholdSystem : BaseSystem
    {
        readonly Family _ribbonFamily = Family.All(typeof(RibbonTrailComponent), typeof(RibbonTrailMovementThresholdComponent), typeof(MovementComponent)).Get();

        readonly ImmutableList<Entity> _ribbonEntities;

        public RibbonTrailMovementThresholdSystem(Engine engine) : base(engine)
        {
            _ribbonEntities = Engine.GetEntitiesFor(_ribbonFamily);
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
            foreach(Entity entity in _ribbonEntities)
            {
                MovementComponent movementComp = entity.GetComponent<MovementComponent>();
                RibbonTrailComponent ribbonTrailComp = entity.GetComponent<RibbonTrailComponent>();
                RibbonTrailMovementThresholdComponent ribbonThreshComp = entity.GetComponent<RibbonTrailMovementThresholdComponent>();

                if(movementComp.MovementVector.LengthSquared()
                    >= ribbonThreshComp.MinimumSpeedForTrail * ribbonThreshComp.MinimumSpeedForTrail)
                {
                    ribbonTrailComp.Starts.Sort();
                    if(ribbonTrailComp.Starts.Count == 0)
                    {
                        ribbonTrailComp.Starts.Add(0);
                        ribbonTrailComp.Ends.Add(0);
                    }
                    if(ribbonTrailComp.Starts[0] == 1)
                    {
                        // We stayed in the threshold
                        ribbonTrailComp.Starts[0] = 0;
                    } else
                    {
                        // Begin new trail
                        ribbonTrailComp.Ends.Add(0);
                        ribbonTrailComp.Starts.Add(0);
                    }
                }
            }
        }
    }
}
