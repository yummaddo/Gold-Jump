using UnityEngine;

namespace Game.GamePlay.Boosts
{
    [CreateAssetMenu(menuName = "Boost/Shield", fileName = "Shield", order = 0)]
    public class Shield : AbstractBoost
    {
        public override void Activate()
        {
            var value = Payment.Instance.GetAmountOfBoost(this);
            if (value > 0)
            {
                if (Session.Instance.playerControl.TryToActiveShieldWithCallbackResult())
                    Payment.Instance.TryUseBoost(this);
            }
        }
    }
}