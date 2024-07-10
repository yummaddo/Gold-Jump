using UnityEngine;

namespace Game.GamePlay
{
    public enum PanelType
    {
        Static, Disappearing, Crash, DoubleJump, Move
    }

    public abstract class AbstractPanel : MonoBehaviour
    {
        protected abstract float GetForceSpeed();
        public PanelType type;
        public abstract void GetForce(Rigidbody2D player);
        public abstract void ContactWithPlayer(Player player);
    }
}