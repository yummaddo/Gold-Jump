using System;
using Game.GamePlay.Services;
using Game.GamePlay.Utility;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Boosts
{
    public class ItemPickUp : MonoBehaviour
    {
        public AbstractBoost boost;
        public ParticleAnimation particleAnimation;
        [Inject] private Payment _payment;
        [Inject] private Player _serviceToLoop;
        private void OnEnable() => particleAnimation.targetOfLifeGameRoot =gameObject;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log($"Pick {boost.name}");
                _payment.AddBoost(boost);
                particleAnimation.PlayWithService(_serviceToLoop);
                Destroy(gameObject);
            }
        }
    }
}