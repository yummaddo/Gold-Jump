using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class WelcomeSession : MonoBehaviour
    {
        public float awaitTime = 4f;
        public void Start()
        {
            Awaiter().Forget();
        }
        private async UniTask Awaiter()
        {
            await UniTask.WaitForSeconds(awaitTime, false, PlayerLoopTiming.Update,
                gameObject.GetCancellationTokenOnDestroy());
            ApplicationContext.Instance.StartFromMenu();
            Destroy(gameObject);
        }
    }
}