using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneReference;

namespace Game.Boot
{
 public class Boot : MonoBehaviour
    {
        // context 
        private ApplicationContext _applicationContext;
        [SerializeField] private Slider Bar;
        [SerializeField] private GameObject BootContext;
        [SerializeField] private TextMeshProUGUI Text;
        private void Awake()
        {
            _applicationContext = ApplicationContext.Instance;
            _applicationContext.OnLoadingScene = ApplicationContextOnLoadingScene;
            _applicationContext.OnSceneLoad = ApplicationContextOnLoadScene;
        }

        private void Start()
        {
            LoadLevel(_applicationContext.LevelsSetting.currentSceneLevel.sceneReference).Forget();
        }
        private async UniTask LoadLevel(SceneReference scene)
        {
            var asyncMenu = await UtilityBoot.LoadSceneAsync(scene, this.GetCancellationTokenOnDestroy(), _applicationContext);
            await UniTask.WaitUntil(() => asyncMenu.isDone);
            _applicationContext.OnSceneLoad?.Invoke();
            await UniTask.WaitForSeconds(0.1f, false, PlayerLoopTiming.Update, _applicationContext.destroyCancellationToken);
            _applicationContext.InitializeNewSession().Forget();
        }
        private void ApplicationContextOnLoadScene()
        {
            Destroy(gameObject);
        }

        private void ApplicationContextOnLoadingScene(float progress)
        {
            try
            {
                Bar.value = Mathf.Clamp01(progress);
                Text.text = ((int)(progress * 100)).ToString()+"%";
#if UNITY_EDITOR
                ClearDeveloperConsole();
#endif
                // Debug.Log($"[Loading...]<color=Green>{progress * 100} %</color>");
            }
            catch (Exception)
            {
                // Debug.Log($"<color=#FF6A38>[Boot Error] {e.Message}</color>");
            }
        }

        private void OnDestroy()
        {
            try
            {
                _applicationContext.OnSceneLoad -= ApplicationContextOnLoadScene;
                _applicationContext.OnLoadingScene -= ApplicationContextOnLoadingScene;
            }
            catch (Exception)
            {
                // ignored
            }
        }
#if UNITY_EDITOR
        private static void ClearDeveloperConsole()
        {
            try
            {
                var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
                if (logEntries != null)
                {
                    var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    if (clearMethod != null) clearMethod.Invoke(null, null);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"<color=#FF6A38>[Boot Error] {e.Message}</color>");
            }
        }
#endif
    }
}