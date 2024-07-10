using System;
using UnityEngine;

namespace Game.GamePlay
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            ClearAll(other);
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            ClearAll(col);
        }
        private static void ClearAll(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Debug.Log("Lose");
                Destroy(col.gameObject);
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