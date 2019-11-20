using GameJam.Common;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using GameJam.Entities;
using Audrey;
using GameJam.Components;

namespace GameJam.Processes
{
    public class CameraProcess : Process
    {
        readonly Family _playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _playerShipList;

        Camera _camera;
        //Vector2 centerOfScreen = new Vector2(0, 0);

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
            float averageX = 0;
            float averageY = 0;
            /*float smallestY = 1000000;
            float smallestX = 1000000;
            float largestY = -1000000;
            float largestX = -1000000;*/

            for (int i = 0; i < _playerShipList.Count; i++)
            {
                averageX += _playerShipList[i].GetComponent<TransformComponent>().Position.X;
                averageY += _playerShipList[i].GetComponent<TransformComponent>().Position.Y;
            }
            averageX /= _playerShipList.Count + 1;
            averageY /= _playerShipList.Count + 1;

            _camera.Position = new Vector2(averageX, averageY);
            Console.WriteLine(_playerShipList.Count);
        }
    }
}
