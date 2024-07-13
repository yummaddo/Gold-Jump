using System;
using Cysharp.Threading.Tasks;
using Game.Core.Abstraction;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GamePlay.Utility
{
    [System.Serializable]
    public class ParticleAnimation
    {
        public GameObject particle;
        public float lifeTime;
        public GameObject targetOfLifeGameRoot;
        public void PlayWithService(AbstractServices service) => Animation(service).Forget();
        private async UniTask Animation(AbstractServices service)
        {
            var time = 0.0f;
            var obj = Object.Instantiate(particle);
            obj.transform.position = targetOfLifeGameRoot.transform.position;
            var particleSystem = obj.GetComponent<ParticleSystem>(); 
            var token = obj.GetCancellationTokenOnDestroy();
            while (!token.IsCancellationRequested && lifeTime >= time)
            {
                time += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
                if (!service.GetStatus())
                    particleSystem.Stop();
                else 
                    if (particleSystem.isStopped) particleSystem.Play();
                await service.BreakPointFixed();
            }
            try
            {
                Object.Destroy(obj);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}