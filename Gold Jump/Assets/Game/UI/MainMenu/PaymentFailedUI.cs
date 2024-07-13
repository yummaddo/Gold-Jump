using System;
using UnityEngine;
using Zenject;

namespace Game.UI.MainMenu
{
    public class PaymentFailedUI : MonoBehaviour
    {
        [Inject] private Payment _payment;
        public GameObject uiElement;
        private void Awake()
        {
            _payment.OnPaymentFailed += Activate;
        }
        private void Activate()
        {
            uiElement.SetActive(true);
        }

        public void DeActivate()
        {
            uiElement.SetActive(false);
        }
    }
}