using System;
using Cysharp.Threading.Tasks;
using Game.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Game.GamePlay
{
    public class Enemy : MonoBehaviour
    {
        public GameObject target;
        [Inject] private Session _session;
        [Inject] private Player _player;
        [Inject] private GameMapService _mapService;
        [Range(1,400)]public float speed = 200;
        [SerializeField] private RectTransform transformUI;
        public float forceMove = 40f;
        private void Awake()
        {
            _session.OnScenePlayerShieldAttack += Dead;
        }
        private void Start()
        {
            MoveMoment().Forget();
        }
        private async UniTask MoveMoment()
        {
            var size = _mapService.uiRect.rect.size;
            float screenWidth = size.x;
            float max = screenWidth / 2;
            float min = -screenWidth / 2;
            bool movingRight = true;
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                Vector3 newPosition = transformUI.localPosition;
                if (movingRight)
                {
                    target.transform.rotation = Quaternion.Euler(0, 180, 0);
                    newPosition.x += speed * Time.fixedDeltaTime;
                    if (newPosition.x >= max)
                    {
                        newPosition.x = max;
                        movingRight = false;
                    }
                }
                else
                {
                    target.transform.rotation = Quaternion.Euler(0, 0, 0);
                    newPosition.x -= speed * Time.fixedDeltaTime;
                    if (newPosition.x <= min)
                    {
                        newPosition.x = min;
                        movingRight = true;
                    }
                }
                transformUI.localPosition = newPosition;
                await _player.BreakPoint();
                await UniTask.WaitForFixedUpdate();
                
            }
        }
        internal void Dead()
        {
            _player.ForceUp(forceMove);
            _mapService.KillEnemy();
            Destroy(target);
        }
    }
}