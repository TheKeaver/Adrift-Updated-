using Audrey;
using GameJam.Input;
using System;

namespace GameJam.Components
{
    /// <summary>
    /// A component for holding a player (an input method).
    /// </summary>
    public class PlayerComponent : IComponent, ICopyComponent
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

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            //TODO Make this work
        }
    }
}
