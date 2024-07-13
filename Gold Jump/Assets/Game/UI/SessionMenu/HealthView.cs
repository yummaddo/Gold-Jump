using System;
using Game.GamePlay.Boosts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.SessionMenu
{
    public class HealthView : MonoBehaviour
    {
        [Inject] private Payment _payment;
        [SerializeField] private SessionMenuControl control;
        public ExtraLife boost;
        public TextMeshProUGUI textMeshProUGUICurrent;
        public TextMeshProUGUI textMeshProUGUICoinsPrice;
        private void OnEnable()
        {
            textMeshProUGUICurrent.text = $"{_payment.GetAmountOfBoost(boost)}";
            textMeshProUGUICoinsPrice.text = $"{boost.Price}";
            _payment.OnPaymentBoostFailed += PaymentBoostPaymentFailed;
            _payment.OnBoostUseConfirm += PaymentBoostPaymentConfirm;
            
        }

        private void PaymentBoostPaymentFailed(AbstractBoost obj)
        {
            if (obj == boost)
                control.ViewCoinFailed();
        }

        private void PaymentBoostPaymentConfirm(AbstractBoost obj)
        {
            if (obj == boost)
            {
                control.ReLife();
            }
        }
        public void ExtraLifeUse()
        {
            _payment.TryUseBoost(boost);
        }
        public void ExtraLifeByAndUse()
        {
            _payment.TryToPayBoost(boost.Price,boost);
            _payment.TryUseBoost(boost);
        }
    }
}