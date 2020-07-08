using Audrey;

namespace GameJam.Components
{
    public class VectorSpriteTrailComponent : IComponent
    {
        public Entity playerShip;
        public float trailWidth;

        public VectorSpriteTrailComponent(Entity ship, float width)
        {
            this.playerShip = ship;
        }
    }
}
