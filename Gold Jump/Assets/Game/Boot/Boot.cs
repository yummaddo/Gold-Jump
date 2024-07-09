using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Boot
{
 public class Boot : MonoBehaviour
    {
        // context 
        private Application _applicationContext;
        [SerializeField] private Scrollbar Bar;
        [SerializeField] private GameObject BootContext;
        private void Awake()
        {
            _applicationContext = Application.Instance;
            Destroy(BootContext);
            DontDestroyOnLoad(gameObject);
            _applicationContext.OnLoadingScene = ApplicationContextOnLoadingScene;
            _applicationContext.OnSceneLoad = ApplicationContextOnLoadScene;
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