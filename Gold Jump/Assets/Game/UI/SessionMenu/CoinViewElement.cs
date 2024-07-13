using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.SessionMenu
{
    public class CoinViewElement : MonoBehaviour
    {
        [Inject] private Payment _payment;
        public TextMeshProUGUI textMeshProUGUI;
        private void OnEnable() => PaymentCoinUpdate();
        private void Awake()
        {
            _payment.OnCoinUpdate += PaymentCoinUpdate;   
        }
        private void PaymentCoinUpdate(int obj) => PaymentCoinUpdate();
        private void PaymentCoinUpdate() => textMeshProUGUI.text = $"{_payment.GetAmountOfCoin()}";
    }
}