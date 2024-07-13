using System;
using UnityEngine;
using Zenject;

namespace Game.GamePlay
{
    public class DeadZone : MonoBehaviour
    {
        [Inject] private Session _session;
        private void OnTriggerStay2D(Collider2D other)
        {
            ClearAll(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            ClearAll(col.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            ClearAll(col.gameObject);
        }
        private void ClearAll(GameObject col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                _session.OnScenePlayerDead?.Invoke();
            }
            if (col.gameObject.CompareTag("PanelStatic"))
                Destroy(col.gameObject);

            else if (col.gameObject.CompareTag("PanelDisappearing"))
                Destroy(col.gameObject);

            else if (col.gameObject.CompareTag("PanelMove"))
                Destroy(col.gameObject);

            else if (col.gameObject.CompareTag("PanelMove"))
                Destroy(col.gameObject);

            else if (col.gameObject.CompareTag("PanelDoubleJump"))
                Destroy(col.gameObject);
        }
    }
}