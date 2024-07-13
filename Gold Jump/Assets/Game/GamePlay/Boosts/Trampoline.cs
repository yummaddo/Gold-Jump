using UnityEngine;

namespace Game.GamePlay.Boosts
{
    [CreateAssetMenu(menuName = "Boost/Trampoline", fileName = "Trampoline", order = 0)]
    public class Trampoline : AbstractBoost
    {
        [Range(1, 500)]public float weight = 60f;
        public override void Activate()
        {
            var value = Payment.Instance.GetAmountOfBoost(this);
            if (value > 0)
            {
                Payment.Instance.TryUseBoost(this);
                Session.Instance.playerControl.ForceUp(weight);
            }
        }
    }
}