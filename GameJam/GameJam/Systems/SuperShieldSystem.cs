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

            if (play.Player.InputMethod.GetSnapshot().SuperShield == true && ship.SuperShieldAvailable && ship.SuperShieldMeter > 0)
            {
                foreach (Entity shield in ship.ShipShields)
                {
                    shield.GetComponent<VectorSpriteComponent>().Hidden = false;
                    shield.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_PLAYER;
                }
                ship.SuperShieldMeter = Math.Max(ship.SuperShieldMeter - CVars.Get<float>("player_super_shield_spend_rate")*dt, 0);
                if (ship.SuperShieldMeter <= 0)
                {
                    ship.SuperShieldAvailable = false;
                    for (int i = 1; i < 4; i++)
                    {
                        ship.ShipShields[i].GetComponent<VectorSpriteComponent>().Hidden = true;
                        ship.ShipShields[i].GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.GROUP_MASK_NONE;
                    }
                }
            }
            else
            {
                ship.SuperShieldMeter = MathHelper.Min(ship.SuperShieldMeter + CVars.Get<float>("player_super_shield_regen_rate") * dt, CVars.Get<float>("player_super_shield_max"));

                if(ship.SuperShieldMeter == CVars.Get<float>("player_super_shield_max"))
                {
                    ship.SuperShieldAvailable = true;
                }
                else
                {
                    ship.SuperShieldAvailable = false;
                }
            }
        }
    }
}
