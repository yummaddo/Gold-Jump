using System;
using UnityEngine;

namespace Game.GamePlay.Utility
{
    public class Teleport: MonoBehaviour
    {
        public RectTransform target;
        public float colduwnTime = 0.5f;
        public void Update() => _time += Time.deltaTime;
        private float _time = 0;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player"))
            {
                return;
            }
            if (_time < colduwnTime) return;
            var transformCollied = col.transform;
            var pos = transformCollied.position;
            pos.x = target.position.x;
            transformCollied.position = pos;
            _time = 0;
        }
    }
}