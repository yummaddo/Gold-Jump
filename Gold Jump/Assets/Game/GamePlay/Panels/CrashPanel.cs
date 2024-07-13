using Game.GamePlay.Panels.Abstraction;
using Game.GamePlay.Utility;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Panels
{
    public class CrashPanel : AbstractPanel
    {
        [Inject] private Player _player;
        public float forceMove = -5f;
        public ParticleAnimation animationParticle;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                ContactWithPlayer(_player);
            }
        }
        protected override float GetForceSpeed()
        {
            return forceMove;
        }
        public override void GetForce(Rigidbody2D player)
        {
        }
        public override void ContactWithPlayer(Player player)
        {
            animationParticle.targetOfLifeGameRoot = this.gameObject;
            animationParticle.PlayWithService(_player);
            Destroy(gameObject);
        }
    }
}