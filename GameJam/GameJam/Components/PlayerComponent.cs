using Audrey;
using GameJam.Input;

namespace GameJam.Components
{
    /// <summary>
    /// A component for holding a player (an input method).
    /// </summary>
    public class PlayerComponent : IComponent
    {
        public Player Player;

        public PlayerComponent(Player player)
        {
            Player = player;
        }

        public InputSnapshot Input
        {
            get
            {
                return Player.InputMethod.GetSnapshot();
            }
        }
    }
}
