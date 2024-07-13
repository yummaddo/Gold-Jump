using UnityEngine;

namespace Game.UI.SessionMenu
{
    public class StartControl : MonoBehaviour
    {
        public GameObject active;
        public GameObject deActive;
        private bool _status = false;
        public void Activate()
        {
            _status = true;
            deActive.SetActive(!_status);
            active.SetActive(_status);
        }
        public void DeActivate()
        {
            _status = false;
            deActive.SetActive(!_status);
            active.SetActive(_status);
        }
    }
}