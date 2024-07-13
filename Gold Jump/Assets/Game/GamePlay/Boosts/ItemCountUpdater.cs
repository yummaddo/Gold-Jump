using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Boosts
{
    public class ItemCountUpdater : MonoBehaviour
    {
        [Inject] private Payment _payment;
        public TextMeshProUGUI textMeshProUGUI;
        public AbstractBoost boots;
        private bool _isInit = false;
        private void Awake()
        {
            _payment.OnBoostUpdate += PaymentBoostUpdate;
            _isInit = true;
        }
        private void PaymentBoostUpdate(AbstractBoost arg1, int arg2)
        {
            if (arg1 == boots) textMeshProUGUI.text = $"{arg2}";
        }
        public void OnEnable()
        {
            textMeshProUGUI.text = $"{_payment.GetAmountOfBoost(boots)}";
            if (!_isInit)
            {
                _isInit = true;
                _payment.OnBoostUpdate += PaymentBoostUpdate;
            }
        }
        public void OnDisable()
        {
            if (_isInit)
            {
                _isInit = false;
                _payment.OnBoostUpdate -= PaymentBoostUpdate;
            }
        }
    }
}