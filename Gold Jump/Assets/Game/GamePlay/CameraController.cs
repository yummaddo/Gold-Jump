using System;
using Game.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Game.GamePlay
{
    public class CameraController : MonoBehaviour
    {
        [Inject] private GameMapService _serviceOfMap;
        public Camera camera;
        public Transform target;
        public void FixedUpdate()
        {
            if (target == null) return;
            if (target.position.y > camera.transform.position.y)
            {
                
                var transformCamera = camera.transform;
                var pos = transformCamera.position;
                pos.y = Mathf.Min(_serviceOfMap.lastHeightPanel, target.position.y);
                transformCamera.position = pos;
            }
        }
    }
}