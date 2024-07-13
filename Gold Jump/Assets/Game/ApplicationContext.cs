using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Boot;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Types;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneReference;
using Zenject;

namespace Game
{
    public class ApplicationContext : MonoBehaviour, IApplication, ILevels, ISetting
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
        private static ApplicationContext _instance;
        internal static ApplicationContext Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<ApplicationContext>();
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
        public void LoadConcrete(LevelScene scene)
        {
            if (LoadNexLevel) AwaiterToJump(scene).Forget();
        }
        public void ReloadLevel() => AwaiterToReload().Forget();
        private async void Start()
        {
            if (FindObjectOfType<MenuSession>())
            {
                LevelsSetting.isWelcome = false;
                return;
            }
            if (!LevelsSetting.isWelcome)
            {
                _session = FindObjectOfType<Session>();
                if (_session == null)
                    await LoadLevel(LevelsSetting.mainMenu);
                else
                    await LoadLevelInCurrentContext();
            }
            else
            {
                LevelsSetting.isWelcome = false;
                SceneManager.LoadScene(LevelsSetting.welcomeScene.SceneName);
            }
        }

        public async void StartFromMenu()
        {
            await LoadLevel(LevelsSetting.mainMenu);
        }
        public void ImmediatelyMenu()
        {
            SceneManager.LoadScene(LevelsSetting.mainMenu.SceneName);
        }
        private async UniTask LoadLevelInCurrentContext()
        {
            _instance = this;
            OnSceneLoad?.Invoke();
            if (LevelsSetting.currentSceneLevel != null)
                await _session.OnAwake(LevelsSetting.currentSceneLevel);
            _session.OnStart();
            await UniTask.CompletedTask;
        }

        public async UniTask LoadLevel(SceneReference scene)
        {
            _instance = this;
            _session = FindObjectOfType<Session>();
            // await to render has been done after get asyncScene with 0.9f progression resource 
            if (_session == null)
            {
                var asyncMenu = await UtilityBoot.LoadSceneAsync(scene, this.GetCancellationTokenOnDestroy(), this);
                await UniTask.WaitUntil(() => asyncMenu.isDone);
                OnSceneLoad?.Invoke();
                await UniTask.WaitForSeconds(0.1f, false, PlayerLoopTiming.Update, this.destroyCancellationToken);
                return;
            }
            var asyncScene = await UtilityBoot.LoadSceneAsync(scene, this.GetCancellationTokenOnDestroy(), this);
            await UniTask.WaitUntil(() => asyncScene.isDone);
            OnSceneLoad?.Invoke()
                ;
            await UniTask.WaitForSeconds(0.2f, false, PlayerLoopTiming.Update, this.destroyCancellationToken);
            if (LevelsSetting.currentSceneLevel != null)
                await _session.OnAwake(LevelsSetting.currentSceneLevel);
            _session.OnStart();
        }

        internal async UniTask InitializeNewSession()
        {
            _session = FindObjectOfType<Session>();
            await _session.OnAwake(LevelsSetting.currentSceneLevel);
            _session.OnStart();
        }

        private void LoadNextLevel()
        {
            // if (Session.Instance.Activity != ActivityType.Restart)
            // {
                LevelsSetting.LoadNext();
                // Session.Instance.Activity = ActivityType.Restart;
                SceneManager.LoadScene(LevelsSetting.bootScene.scene.SceneName, LoadSceneMode.Single);
                // LoadLevel(LevelsSetting.currentSceneLevel.sceneReference).Forget();
            // } 
        }
        private void LoadNextConcrete(LevelScene scene)
        {
            // if (Session.Instance.Activity != ActivityType.Restart)
            // {
                LevelsSetting.LoadConcrete(scene);
                // Session.Instance.Activity = ActivityType.Restart;
                SceneManager.LoadScene(LevelsSetting.bootScene.scene.SceneName, LoadSceneMode.Single);
                // LoadLevel(LevelsSetting.currentSceneLevel.sceneReference).Forget();
            // } 
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
        private async UniTask AwaiterToJump(LevelScene scene)
        {
            OnLevelLoadAwaitStart?.Invoke();
            // await UniTask.WaitForSeconds(LoadFadeTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            await UniTask.WaitForSeconds(LoadNexLevelTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            OnLevelLoadAwaitEnd?.Invoke();
            LoadNextConcrete(scene);
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
