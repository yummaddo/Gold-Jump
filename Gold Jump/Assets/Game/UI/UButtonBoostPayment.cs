using Game.GamePlay.Boosts;
using UnityEngine;

namespace Game.UI
{
    public class UButtonBoostPayment : MonoBehaviour
    {
        public AbstractBoost boost;
        public void PaymentProvide() => boost.PaymentProvide();
    }
}