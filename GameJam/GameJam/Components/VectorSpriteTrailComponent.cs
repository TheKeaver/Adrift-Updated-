using Audrey;

namespace GameJam.Components
{
    public class VectorSpriteTrailComponent : IComponent
    {
        public Entity playerShip;

        public VectorSpriteTrailComponent(Entity ship)
        {
            this.playerShip = ship;
        }
    }
}
