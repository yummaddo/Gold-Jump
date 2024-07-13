using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Abstraction;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneReference;
namespace Game.Boot
{
    public static class UtilityBoot
    {
        internal static async UniTask<AsyncOperation> LoadSceneAsync(SceneReference scene, CancellationToken token, IApplication context)
        {
            var staticLoadTime = context.StaticLoadTime;
            var totalLoadTime = 0.0f;
            if (scene == null) { 
                Debug.LogError("Current SceneAsset is null");
                return null;
            }
            string sceneName = scene.SceneName;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f &&!token.IsCancellationRequested)
            {
                await UniTask.Yield(token);
                context.OnLoadingScene?.Invoke(Mathf.Clamp(totalLoadTime/staticLoadTime,0.0f, 0.5f));
                totalLoadTime += Time.deltaTime;
            }
            while (totalLoadTime < staticLoadTime)
            {
                context.OnLoadingScene?.Invoke(Mathf.Clamp(totalLoadTime/staticLoadTime,0.0f, 0.95f));
                await UniTask.Yield(token);
                totalLoadTime += Time.deltaTime;
            }
            await UniTask.Yield(token);
            context.OnLoadingScene?.Invoke(Mathf.Clamp(0.0f,1f,totalLoadTime/staticLoadTime));
            asyncOperation.allowSceneActivation = true;
            return asyncOperation;
        }
    }
}