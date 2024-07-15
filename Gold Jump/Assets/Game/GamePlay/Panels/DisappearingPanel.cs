using Game.GamePlay.Panels.Abstraction;
using Game.GamePlay.Utility;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Panels
{
    public class DisappearingPanel : AbstractPanel
    {
        [Inject] private Player _player;
        public float forceMove = 10f;
        public ParticleAnimation animationParticle;
        protected override float GetForceSpeed()
        {
            return forceMove;
        }
        public override void GetForce(Rigidbody2D player)
        {
            if ( player.velocity.y < GetForceSpeed()/2f) player.velocity = Vector2.up * GetForceSpeed();

        }
        public override void ContactWithPlayer(Player player)
        {
            animationParticle.targetOfLifeGameRoot = this.gameObject;
            animationParticle.PlayWithService(_player);
            Destroy(gameObject);
        }
    }
}