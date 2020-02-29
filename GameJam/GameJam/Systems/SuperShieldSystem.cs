using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class SuperShieldSystem : BaseSystem
    {
        readonly Family playerFamily = Family.All(typeof(PlayerComponent), typeof(PlayerShipComponent)).Get();
        readonly ImmutableList<Entity> playerEntities;

        public SuperShieldSystem(Engine engine) : base(engine)
        {
            playerEntities = engine.GetEntitiesFor(playerFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity player in playerEntities)
            {
                UpdateSuperShieldFromInput(player, dt);
            }
        }

        private void UpdateSuperShieldFromInput(Entity player, float dt)
        {
            PlayerComponent play = player.GetComponent<PlayerComponent>();
            PlayerShipComponent ship = player.GetComponent<PlayerShipComponent>();

            if (play.Player.InputMethod.GetSnapshot().SuperShield == true)
            {
                if (ship.SuperShieldMeter > 0)
                {
                    ship.SuperShieldMeter -= CVars.Get<float>("player_super_shield") * dt;
                }
            }
            else
                ship.SuperShieldMeter = MathHelper.Min(ship.SuperShieldMeter + CVars.Get<float>("player_super_shield_regen_rate") * dt, CVars.Get<float>("player_super_shield_min"));
        }
    }
}
