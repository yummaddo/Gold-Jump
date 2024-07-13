using System;
using System.Collections;
using Game.Core.Types;
using UnityEngine;
using Zenject;

namespace Game.Core.Abstraction
{
public enum ServiceStatus
    {
        Pause, Play, Boot
    }
    
    public abstract class AbstractServices : AbstractServiceBoot, IService, IStopSession
    {
        [field: SerializeField] internal ActivityType[] Expected
        {
            get; 
            private set;
        } = new ActivityType[] { ActivityType.Playing };
        [field:SerializeField] private ServiceStatus status = ServiceStatus.Play;
        internal Action OnStartService;
        internal Action OnStopService;
        internal Action OnReStartService;
        internal Action OnAwakeService;
        internal Session Session;

        [Inject] protected void Construct(Session sessionManager)
        {
            Session = sessionManager;
            Session.OnSceneAwakeEndedSession += OnAwakeCaller;
            Session.OnSceneStartSession += OnStartCaller;
            Session.OnReStartSession += ReStart;
            Session.OnStopSession += Stop;
        }
        internal bool GetStatus() => Active();
        internal bool GetExpectationStatus() => Active() && ((IList)Expected).Contains(Session.Activity);
        protected override bool Active() => status == ServiceStatus.Play;
        protected virtual void Freezing() { }
        protected virtual void UnFreezing() { }
        
        public void OnAwakeCaller()
        {
            OnAwake();
            OnAwakeService?.Invoke();
        }
        public void OnStartCaller()
        {
            OnStart();
            OnStartService?.Invoke();
        }
        private void OnDestroy()
        {
            Session.OnSceneAwakeEndedSession -= OnAwakeCaller;
            Session.OnSceneStartSession -= OnStartCaller;
        }
        public void Stop()
        {
            status = ServiceStatus.Pause;
            OnStopService?.Invoke();
        }
        public void ReStart()
        {
            status = ServiceStatus.Play;
            OnReStartService?.Invoke();
        }
        
    }
}