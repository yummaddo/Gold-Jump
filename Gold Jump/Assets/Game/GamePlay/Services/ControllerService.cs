using System;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Abstraction;
using UnityEngine;

namespace Game.GamePlay.Services
{
    public class ControllerService : AbstractServices
    {
        private float _horizontalSpeed;
        public float HorizontalSpeed
        {
            get => _horizontalSpeed;
        }
        public override void OnAwake() { }
        public override void OnStart() { }

        private void Update()
        {
    
            
            
#if UNITY_EDITOR
            if (!Active()) return;
            if (Input.GetKeyDown("a"))
                _horizontalSpeed = -1f/3;
            if (Input.GetKeyDown("d"))
                _horizontalSpeed = 1f/3;
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
                _horizontalSpeed = 0f;

#elif UNITY_IOS || UNITY_ANDROID
            _horizontalSpeed = Input.acceleration.x;
#else
            if (!Active()) return;
            if (Input.GetKeyDown("a"))
                _horizontalSpeed = -1f/3;
            if (Input.GetKeyDown("d"))
                _horizontalSpeed = 1f/3;
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
                _horizontalSpeed = 0f;
#endif
        }
    }
}