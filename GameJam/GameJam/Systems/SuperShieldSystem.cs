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

        readonly Family superShieldFamiliy = Family.All(typeof(SuperShieldComponent), typeof(VectorSpriteComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> superShields;

        public SuperShieldSystem(Engine engine) : base(engine)
        {
            playerEntities = engine.GetEntitiesFor(playerFamily);
            superShields = engine.GetEntitiesFor(superShieldFamiliy);
        }

        public void Update(float dt)
        {
            foreach (Entity player in playerEntities)
            {
                UpdateSuperShieldFromInput(player, dt);
            }
            foreach(Entity superShield in superShields)
            {
                UpdateLocationInfo(superShield);
            }
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnKill()
        {
            return;
        }

        protected override void OnTogglePause()
        {
            return;
        }

        protected override void OnUpdate(float dt)
        {
            Update(dt);
        }

        private void UpdateLocationInfo(Entity shield)
        {
            SuperShieldComponent ssc = shield.GetComponent<SuperShieldComponent>();

            //shield.GetComponent<TransformComponent>().SetPosition(ssc.ship.GetComponent<TransformComponent>().Position);
            //shield.GetComponent<TransformComponent>().SetRotation(ssc.ship.GetComponent<TransformComponent>().Rotation);
            shield.GetComponent<VectorSpriteComponent>().Alpha = ssc.ship.GetComponent<PlayerShipComponent>().SuperShieldMeter / CVars.Get<float>("player_super_shield_max");
        }

        private void UpdateSuperShieldFromInput(Entity player, float dt)
        {
            PlayerComponent play = player.GetComponent<PlayerComponent>();
            PlayerShipComponent ship = player.GetComponent<PlayerShipComponent>();

            if (play.Player.InputMethod.GetSnapshot().SuperShield == true && ship.SuperShieldAvailable && ship.SuperShieldMeter > 0)
            {
                foreach (Entity shield in ship.ShipShields)
                {
                    //shield.GetComponent<PlayerShieldComponent>().LaserReflectionActive = true;
                    shield.GetComponent<VectorSpriteComponent>().Hidden = false;
                    shield.GetComponent<CollisionComponent>().CollisionMask = (byte)(Constants.Collision.COLLISION_GROUP_ENEMIES | Constants.Collision.COLLISION_GROUP_RAYCAST);
                    shield.GetComponent<CollisionComponent>().CollisionGroup = (byte)(Constants.Collision.COLLISION_GROUP_PLAYER);
                }
                ship.SuperShieldMeter = Math.Max(ship.SuperShieldMeter - CVars.Get<float>("player_super_shield_spend_rate")*dt, 0);
                //Console.WriteLine("Super shield meter at " + ship.SuperShieldMeter);
            }
            else
            {
                if (ship.SuperShieldMeter <= 0 || play.Player.InputMethod.GetSnapshot().SuperShield == false)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        //ship.ShipShields[i].GetComponent<PlayerShieldComponent>().LaserReflectionActive = false;
                        ship.ShipShields[i].GetComponent<VectorSpriteComponent>().Hidden = true;
                        ship.ShipShields[i].GetComponent<CollisionComponent>().CollisionMask = Constants.Collision.GROUP_MASK_NONE;
                        ship.ShipShields[i].GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.GROUP_MASK_NONE;
                        //Console.WriteLine("Super Shield collision mask set to NONE");
                    }
                }
                ship.SuperShieldMeter = MathHelper.Min(ship.SuperShieldMeter + CVars.Get<float>("player_super_shield_regen_rate") * dt, CVars.Get<float>("player_super_shield_max"));
                //Console.WriteLine("Super shield at " + ship.SuperShieldMeter);
                if(ship.SuperShieldMeter == CVars.Get<float>("player_super_shield_max"))
                {
                    //Console.WriteLine("Super shield resource replenished");
                    ship.SuperShieldAvailable = true;
                }
                else
                {
                    ship.SuperShieldAvailable = false;
                    //Console.WriteLine("Player super shield out of resource");
                }
            }
        }
    }
}
