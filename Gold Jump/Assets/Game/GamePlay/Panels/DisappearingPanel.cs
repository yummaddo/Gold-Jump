using UnityEngine;

namespace Game.GamePlay
{
    public class DisappearingPanel : AbstractPanel
    {
        public float forceMove = 10f;
        protected override float GetForceSpeed()
        {
            return forceMove;
        }
        public override void GetForce(Rigidbody2D player)
        {
            player.velocity = Vector2.up * GetForceSpeed();
        }
        public override void ContactWithPlayer(Player player)
        {
        }
    }
}