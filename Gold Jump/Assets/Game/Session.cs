using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Types;
using UnityEngine;
using Zenject;

namespace Game
{
    internal enum SessionState
    {
        Active,DeActive,Dead,Win
    }

    public class Session : MonoBehaviour
    {
        [Inject] private Application _application;
        [Inject] private DiContainer _container;
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

        internal Action OnStopSession;
        internal Action OnReStartSession;
        
        private Action _onScenePlayerLoadNextLevelAfterWon;
        internal Action OnScenePlayerWon;

        internal bool Active => _status == SessionState.Active;
        private SessionState _status = SessionState.Active;
        
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
        [field: SerializeField] private LevelScene setting { get; set; }
        private void Awake()
        {
            _onScenePlayerLoadNextLevelAfterWon += _application.OnGameWin;
            _instance = this;
        }
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
            setting = scene;
            Activity = ActivityType.Boot;
            OnSceneBootSession?.Invoke(); // boot end
            await UniTask.WaitForSeconds(BootTime, false, PlayerLoopTiming.Update, destroyCancellationToken);
            OnSceneAwakeEndedSession?.Invoke(); // awake end
            await UniTask.WaitForFixedUpdate();
        }
        internal void OnStart()
        {
            OnSceneStartSession?.Invoke();
        }
        private void OnDisable()
        {
            OnScenePlayerDead -= () => { Activity = ActivityType.Dead; };
            OnScenePlayerWon -= () => { Activity = ActivityType.Winning; };
        }
    }
}
