using Game.GamePlay.Boosts;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UButtonBoostPayment : MonoBehaviour
    {
        public AbstractBoost boost;
        public TextMeshProUGUI textOfPrice;
        public void PaymentProvide() => boost.PaymentProvide();
        private void OnEnable() => textOfPrice.text = boost.Price.ToString();
    }
}