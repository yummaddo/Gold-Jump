using Game.GamePlay.Panels.Abstraction;
using UnityEngine;

namespace Game.GamePlay.Panels
{
    public class DoubleJumpPanel : AbstractPanel
    {
        public float forceMove = 20f;
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
        }
    }
}