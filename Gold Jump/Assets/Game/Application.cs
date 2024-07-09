using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Boot;
using Game.Core;
using Game.Core.Types;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneReference;
using Zenject;

namespace Game
{
    public class Application : MonoBehaviour, IApplication, ILevels, ISetting
    {
        public Action<float> OnLoadingScene { get; set; }
        public Action OnSceneLoad { get; set; }
        
        public Action OnLevelLoadAwaitStart { get; set; }
        public Action OnLevelLoadAwaitEnd { get; set; }
        
        public Action OnGameEnd { get; set; }
        public Action OnGameWin { get; set; }
        public Action OnGameLose { get; set; }
        public Action OnSessionFadeOff { get; set; }
        private Session _session;

        [field: Header("Application")]
        [field: SerializeField] public float StaticLoadTime { get; set; } = 2f;

        [field: Header("Setting")]
        [field: SerializeField] public bool LoadNexLevel { get; set; } = false;
        [field: SerializeField] public bool GoadAlivePlayer { get; set; } = false;
        
        [field: SerializeField] public float LoadNexLevelTime { get; set; } = 1f;
        [field: SerializeField] public float ReloadSceneTime { get; set; } = 1f;
        
        [field: Header("Levels set")]
        [field: SerializeField] public ApplicationSetting LevelsSetting { get; set; }
        private static Application _instance;
        internal static Application Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<Application>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }

        public void LoadNext()
        {
            if (LoadNexLevel) AwaiterToJump().Forget();
        }
        public void ReloadLevel() => AwaiterToReload().Forget();

        private async void Start()
        {
            await LoadLevel(LevelsSetting.mainMenu);
        }
        private async UniTask LoadLevel(SceneReference scene)
        {
            _instance = this;
            _session = FindObjectOfType<Session>();
            var asyncScene = await UtilityBoot.LoadSceneAsync(scene, this.GetCancellationTokenOnDestroy(), this);
            // TODO await to render has been done after get asyncScene with 0.9f progression resource 
            await UniTask.WaitUntil(() => asyncScene.isDone);
            OnSceneLoad?.Invoke();
            await UniTask.WaitForSeconds(0.2f, false, PlayerLoopTiming.Update, this.destroyCancellationToken);
            await _session.OnAwake(LevelsSetting.currentSceneLevel);
            _session.OnStart();
        }
        private void LoadNextLevel()
        {
            if (Session.Instance.Activity != ActivityType.Restart)
            {
                var id = LevelsSetting.currentSceneLevel.id;
                LevelsSetting.LoadNext();
                Session.Instance.Activity = ActivityType.Restart;
                SceneManager.LoadScene(LevelsSetting.bootScene.scene.SceneName, LoadSceneMode.Single);
                LoadLevel(LevelsSetting.currentSceneLevel.sceneReference).Forget();
            } 
        }
        private void ReLoadLevel()
        {
            if (Session.Instance.Activity != ActivityType.Restart)
            {
                Session.Instance.Activity = ActivityType.Restart;
                SceneManager.LoadScene(LevelsSetting.bootScene.scene.SceneName, LoadSceneMode.Single);
                LoadLevel(LevelsSetting.currentSceneLevel.sceneReference).Forget();
            }
        }
        /// <summary>
        /// TODO: Load next level 
        /// </summary>
        private async UniTask AwaiterToJump()
        {
            OnLevelLoadAwaitStart?.Invoke();
            // await UniTask.WaitForSeconds(LoadFadeTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            await UniTask.WaitForSeconds(LoadNexLevelTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            OnLevelLoadAwaitEnd?.Invoke();
            LoadNextLevel();
        }
        /// <summary>
        /// TODO : Reload current level
        /// </summary>
        private async UniTask AwaiterToReload()
        {
            OnLevelLoadAwaitStart?.Invoke();
            // await UniTask.WaitForSeconds(LoadFadeTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            await UniTask.WaitForSeconds(ReloadSceneTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            OnLevelLoadAwaitEnd?.Invoke();
            ReLoadLevel();
        }
    }
}
