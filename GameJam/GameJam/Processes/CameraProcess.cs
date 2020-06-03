using GameJam.Common;
using System;
using Microsoft.Xna.Framework;
using Audrey;
using GameJam.Components;

namespace GameJam.Processes
{
    public class CameraProcess : Process
    {
        readonly Family _playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _playerShipList;

        Camera _camera;

        public CameraProcess(Camera camera, Engine sharedEngine)
        {
            _camera = camera;
            _playerShipList = sharedEngine.GetEntitiesFor(_playerShipFamily);
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
            if (CVars.Get<bool>("debug_enable_camera_movement"))
            {
                float camPad = CVars.Get<float>("camera_padding");
                float averageX = 0;
                float averageY = 0;

                float Z = 1;

                for (int i = 0; i < _playerShipList.Count; i++)
                {
                    float tempShipX = _playerShipList[i].GetComponent<TransformComponent>().Position.X;
                    float tempShipY = _playerShipList[i].GetComponent<TransformComponent>().Position.Y;

                    averageX += tempShipX;
                    averageY += tempShipY;
                }

                averageX /= _playerShipList.Count + 1;
                averageY /= _playerShipList.Count + 1;

                Vector2 targetPosition = new Vector2(averageX, averageY);
                _camera.Position = Vector2.Lerp(_camera.Position, targetPosition, dt * 144 * CVars.Get<float>("camera_tracking_speed"));


                for (int i = 0; i < _playerShipList.Count; i++)
                {
                    float tempShipX = _playerShipList[i].GetComponent<TransformComponent>().Position.X;
                    float tempShipY = _playerShipList[i].GetComponent<TransformComponent>().Position.Y;

                    float maxDistX = Math.Abs(tempShipX - _camera.Position.X) + camPad;
                    float maxDistY = Math.Abs(tempShipY - _camera.Position.Y) + camPad;
                    //float maxDistX = camPad + tempShipX - _camera.Position.X;// + camPad;
                    //float maxDistY = camPad + tempShipY - _camera.Position.Y;// + camPad;

                    if (maxDistX > CVars.Get<float>("screen_width") / 2
                        || maxDistY > CVars.Get<float>("screen_height") / 2 || true)
                    {
                        float tempZX_2 = maxDistX / (CVars.Get<float>("screen_width") / 2);
                        float tempZY_2 = maxDistY / (CVars.Get<float>("screen_height") / 2);
                        if (tempZX_2 > Z)
                        {
                            Z = tempZX_2;
                        }
                        if (tempZY_2 > Z)
                        {
                            Z = tempZY_2;
                        }
                    }
                    float targetZoom = 1 / Z;
                    _camera.Zoom = MathHelper.Lerp(_camera.Zoom, targetZoom, 0.01f);
                }
            }
        }
    }
}
