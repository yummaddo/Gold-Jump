using System;
using System.Security.Cryptography;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Abstraction;
using Game.GamePlay.Panels.Abstraction;
using Game.GamePlay.Services;
using Game.GamePlay.Utility;
using UnityEngine;
using Zenject;

namespace Game.GamePlay
{
    public class Player : AbstractServices
    {
        [Inject] private ControllerService _serviceController;
        [Inject] private Session _session;
        public ParticleAnimation shieldBoom;
        [SerializeField] private Rigidbody2D bodyGravity;
        [SerializeField] private GameObject shieldEffect;
        public float moveSpeedWidth = 5f;
        public bool shieldActive = false;
        public float upValueOnReLife = 30;
        public float forceValueOnReLife = 250;
        
        public override void OnAwake()
        {
            bodyGravity.simulated = false;
            _session.OnScenePlayerReLife += ReLifePlayer;
        }

        private void ReLifePlayer()
        {
            try
            {
                bodyGravity.simulated = true;
                transform.position += Vector3.up * upValueOnReLife;
                ForceUp(forceValueOnReLife);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public override void OnStart()
        {
            bodyGravity.simulated = false;
            MoveProcedure().Forget();
        }

        internal void ForceUp(float value) => bodyGravity.velocity = Vector2.up * value;
        public bool TryToActiveShieldWithCallbackResult()
        {
            if (shieldActive)
            {
                return false;
            }
            else
            {
                shieldActive = true;
                return true;
            }
        }

        private async UniTask MoveProcedure()
        {
            bodyGravity.simulated = true;
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                if (!Active())
                {
                    bodyGravity.simulated = false;
                    await UniTask.WaitUntil(Active);
                    bodyGravity.simulated = true;
                }
                await UniTask.WaitForFixedUpdate();
                if (destroyCancellationToken.IsCancellationRequested) return;
                bodyGravity.velocity = new Vector2(_serviceController.HorizontalSpeed*moveSpeedWidth,bodyGravity.velocity.y);
                shieldEffect.SetActive(shieldActive);
            }
        }
        private void OnCollisionEnter2D(Collision2D col)
        {
            EnterCollision(col);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Finish"))
            {
                this.gameObject.SetActive(false);
                _session.OnScenePlayerWon?.Invoke();
            } 
            else if (col.gameObject.CompareTag("DeadZoneEnemy"))
            {
                if (shieldActive)
                {
                    _session.OnScenePlayerShieldAttack?.Invoke();
                    shieldBoom.targetOfLifeGameRoot = this.gameObject;
                    shieldBoom.PlayWithService(this);
                    shieldActive = false;
                }
                else
                {
                    _session.OnScenePlayerDead?.Invoke();
                }
            }
            else if (col.gameObject.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>().Dead();
            }
        }

        private void EnterCollision(Collision2D other)
        {
            if (other.collider.gameObject.CompareTag("PanelStatic"))
                JumpProcedure(other.gameObject.GetComponent<AbstractPanel>()).Forget();
            else if (other.collider.gameObject.CompareTag("PanelDisappearing"))
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