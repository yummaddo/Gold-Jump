using System;
using UnityEngine;

namespace Game.GamePlay.Utility
{
    public class Teleport: MonoBehaviour
    {
        public RectTransform target;
        private void OnTriggerEnter2D(Collider2D col)
        {
            var transformCollied = col.transform;
            var pos = transformCollied.position;
            pos.x = target.position.x;
            transformCollied.position = pos;
        }
    }
}