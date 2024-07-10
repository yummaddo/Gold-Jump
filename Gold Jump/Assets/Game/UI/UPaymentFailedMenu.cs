using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class UPaymentFailedMenu : MonoBehaviour
    {
        public RectTransform target;
        [Inject]
        public void Construct(Payment payment)
        {
            payment.OnPaymentFailed += OnPaymentFailed;
        }

        private void OnPaymentFailed()
        {
            target.gameObject.SetActive(true);
        }
    }
}