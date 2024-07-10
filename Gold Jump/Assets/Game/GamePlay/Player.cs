using System;
using System.Security.Cryptography;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Game.GamePlay
{
    public class Player : MonoBehaviour
    {
        [Inject] private ControllerService _serviceController;
        public float moveSpeedWidth = 5f;
        [SerializeField] private Rigidbody2D bodyGravity;
        private void FixedUpdate()
        {
            bodyGravity.velocity = new Vector2(_serviceController.HorizontalSpeed*moveSpeedWidth,bodyGravity.velocity.y);
        }
        private void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log(col.collider.gameObject.name);
            EnterCollision(col);
        }
        private void EnterCollision(Collision2D other)
        {
            if (other.collider.gameObject.CompareTag("PanelStatic"))
                JumpProcedure(other.gameObject.GetComponent<AbstractPanel>()).Forget();
            else if (other.collider.gameObject.CompareTag("PanelDisappearing"))
                JumpProcedure(other.gameObject.GetComponent<AbstractPanel>()).Forget();
            else if (other.collider.gameObject.CompareTag("PanelMove"))
                JumpProcedure(other.gameObject.GetComponent<AbstractPanel>()).Forget();
            else if (other.collider.gameObject.CompareTag("PanelMove"))
                JumpProcedure(other.gameObject.GetComponent<AbstractPanel>()).Forget();
            else if (other.collider.gameObject.CompareTag("PanelDoubleJump"))
                JumpProcedure(other.gameObject.GetComponent<AbstractPanel>()).Forget();
        }
        private async UniTask JumpProcedure(AbstractPanel abstractPanel)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
            abstractPanel.ContactWithPlayer(this);
            abstractPanel.GetForce(bodyGravity);
            // bodyGravity.velocity = Vector2.up * moveSpeed;
            await UniTask.CompletedTask;
        }
    }
}