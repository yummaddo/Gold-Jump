using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.MainMenu
{
    public class BonusConfirm : MonoBehaviour
    {
        [Inject] private Payment _payment;
        [SerializeField] private MenuControl control;
        [SerializeField] private int bonus = 60;
        public TextMeshProUGUI textMeshProUGUI;
        private void OnEnable()
        {
            textMeshProUGUI.text = bonus.ToString();
        }
        private void OnDisable()
        {
            _payment.AddCoin(bonus);
        }
        public void BonusConfirmAction()
        {
            control.last = MenuType.Main;
            control.OpenMain();
        }
    }
}