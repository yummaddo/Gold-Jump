using System;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Types;
using Game.GamePlay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    internal enum SessionState
    {
        Active,DeActive,Dead,Win
    }

    public class Session : MonoBehaviour
    {
        [Inject] private ApplicationContext _application;
        [Inject] private DiContainer _container;
        [Inject] internal Player playerControl;
        
        [field: SerializeField] private float BootTime { get; set; } = 0.2f;
        [field: SerializeField] internal int SessionCoins { get; set; } = 0;
        [field: SerializeField] internal ActivityType Activity { get; set; } = ActivityType.Boot;
        
        internal Action OnSceneBootSession;
        internal Action OnSceneGenerationSession;
        internal Action OnSceneAwakeEndedSession;
        internal Action OnSceneStartSession;
        internal Action OnSceneEndSession;
        
        internal Action OnScenePlayerDead; // when dead in Level
        internal Action OnScenePlayerTutorialDead; // dead in tutorial state
        internal Action OnScenePlayerShieldAttack; // when dead in Level

        
        internal Action OnStopSession;
        internal Action OnReStartSession;
        
        private Action _onScenePlayerLoadNextLevelAfterWon;
        internal Action OnScenePlayerWon;
        internal Action OnScenePlayerReLife;

        internal bool Active => _status == SessionState.Active;
        private SessionState _status = SessionState.Active;
        public Image backGround;
        
        private void Awake()
        {
            OnScenePlayerDead += ScenePlayerDead;
            OnScenePlayerWon += ScenePlayerWon;
            _onScenePlayerLoadNextLevelAfterWon += _application.OnGameWin;
            _instance = this;
        }

        private void ScenePlayerDead()
        {
            _status = SessionState.Dead;
            OnStopSession?.Invoke();
        }
        
        internal void ScenePlayerReLife()
        {
            _status = SessionState.Active;
            OnReStartSession?.Invoke();
            OnScenePlayerReLife?.Invoke();
        }
        private void ScenePlayerWon()
        {
            _status = SessionState.Win;
            OnStopSession?.Invoke();
        }
        public void SessionStop()
        {
            _status = SessionState.DeActive;
            OnStopSession?.Invoke();
        }
        public void SessionReActivate()
        {
            _status = SessionState.Active;
            OnReStartSession?.Invoke();
        }
        
        internal void TryToLoadNext()
        {
            if (Activity == ActivityType.Winning)
            {
                _onScenePlayerLoadNextLevelAfterWon?.Invoke();
            }
        }
        [field: SerializeField] private LevelScene Setting { get; set; }

        private static Session _instance;
        public static Session Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<Session>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        internal async UniTask OnAwake(LevelScene scene) // scene context generation and initialization
        {
            Setting = scene;
            Activity = ActivityType.Boot;
            OnSceneBootSession?.Invoke(); // boot end
            await UniTask.WaitForSeconds(BootTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            OnSceneAwakeEndedSession?.Invoke(); // awake end
            await UniTask.WaitForFixedUpdate();
        }
        internal async UniTask OnAwake() // scene context generation and initialization
        {
            Activity = ActivityType.Boot;
            OnSceneBootSession?.Invoke(); // boot end
            await UniTask.WaitForSeconds(BootTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            OnSceneAwakeEndedSession?.Invoke(); // awake end
            await UniTask.WaitForFixedUpdate();
        }
        internal void OnStart()
        {
            backGround.sprite = Setting.backGround;
            OnSceneStartSession?.Invoke();
            Activity = ActivityType.Playing;
        }
        private void OnDisable()
        {
            OnScenePlayerDead -= () => { Activity = ActivityType.Dead; };
            OnScenePlayerWon -= () => { Activity = ActivityType.Winning; };
        }
    }
}
