using Cysharp.Threading.Tasks;
using Game.GamePlay.Panels.Abstraction;
using Game.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Panels
{
    public class MovePanel : AbstractPanel
    {
        [Inject] private Player _player;
        [Inject] private GameMapService _gameMapService;
        [Range(1,200)]public float speed = 3;
        public float forceMove = 40f;
        private void Start()
        {
            MoveMoment().Forget();
        }
        private async UniTask MoveMoment()
        {
            var size = _gameMapService.uiRect.rect.size;
            float screenWidth = size.x;
            float max = screenWidth / 2;
            float min = -screenWidth / 2;
            bool movingRight = true;
            RectTransform transformUI = GetComponent<RectTransform>();
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                Vector3 newPosition = transformUI.localPosition;
                if (movingRight)
                {
                    newPosition.x += speed * Time.fixedDeltaTime;
                    if (newPosition.x >= max)
                    {
                        newPosition.x = max;
                        movingRight = false;
                    }
                }
                else
                {
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
        protected override float GetForceSpeed()
        {
            return forceMove;
        }
        public override void GetForce(Rigidbody2D player)
        {
            if ( player.velocity.y < GetForceSpeed()/2f) player.velocity = Vector2.up * GetForceSpeed();
        }
        public override void ContactWithPlayer(Player player)
        {
        }
    }
}