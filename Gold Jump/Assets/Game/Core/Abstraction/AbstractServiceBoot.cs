using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Types;
using UnityEngine;

namespace Game.Core
{
public abstract class AbstractServiceBoot : MonoBehaviour, IServiceBoot
    {
        protected abstract bool Active();
        internal virtual bool ServiceActive(ActivityType[] expect, Session manager)
        {
            return ((IList)expect).Contains(manager.Activity);
        }
        public abstract void OnAwake();
        public abstract void OnStart();
        
        internal async UniTask BreakPointFixed()
        {
            if (!Active()) await UniTask.WaitUntil(Active);
        }
        internal async UniTask BreakPoint()
        {
            if (!Active()) await UniTask.WaitUntil(Active);
        }
        internal async UniTask BreakPoint(ActivityType expect, Session manager)
        {
            if (!Active()) await UniTask.WaitUntil(Active);
            await  UniTask.WaitUntil( () => expect == manager.Activity);
        }
        internal async UniTask BreakPoint(ActivityType[] expect, Session manager)
        {
            if (!Active()) await UniTask.WaitUntil(Active);
            await  UniTask.WaitUntil( () => ((IList)expect).Contains(manager.Activity));
        }
        internal async UniTask BreakPoint(float time)
        {
            var elipsilonTime = 0.0f;
            while (elipsilonTime < time)
            {
                try
                {
                    await UniTask.WaitForFixedUpdate(destroyCancellationToken);
                    elipsilonTime += Time.fixedDeltaTime;
                    await BreakPoint();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
        internal async UniTask BreakPoint(float time, ActivityType expect, Session manager)
        {
            var elipsilonTime = 0.0f;
            while (elipsilonTime < time)
            {
                await UniTask.WaitForFixedUpdate(destroyCancellationToken);
                elipsilonTime += Time.fixedDeltaTime;
                await BreakPoint(expect, manager);
            }
        }
        internal async UniTask BreakPoint(float time,ActivityType[] expect, Session manager)
        {
            var elipsilonTime = 0.0f;
            while (elipsilonTime < time)
            {
                await UniTask.WaitForFixedUpdate(destroyCancellationToken);
                elipsilonTime += Time.fixedDeltaTime;
                await BreakPoint(expect, manager);
            }
        }
        internal async UniTask<float> BreakPointInUpdate(float time,ActivityType[] expect, Session manager)
        {
            var epsilonTime = 0.0f;
            while (epsilonTime < time)
            {
                await UniTask.Yield(destroyCancellationToken);
                epsilonTime += Time.deltaTime;
                await BreakPoint(expect, manager);
            }
            return epsilonTime;
        }
        internal async UniTask BreakPoint(float time,ActivityType[] expect, Session manager, CancellationToken token)
        {
            var elipsilonTime = 0.0f;
            while (elipsilonTime < time && !token.IsCancellationRequested)
            {
                await UniTask.WaitForFixedUpdate(token);
                elipsilonTime += Time.fixedDeltaTime;
                await BreakPoint(expect, manager);
            }
        }
        protected async UniTask BreakPoint(PlayerLoopTiming timing)
        {
            if (!Active()) await UniTask.WaitUntil(Active, timing,destroyCancellationToken);
            else { await UniTask.Yield(timing,destroyCancellationToken); }
        }
        protected async UniTask BreakPoint(PlayerLoopTiming timing, CancellationToken token)
        {
            if (!Active()) await UniTask.WaitUntil(Active,timing,token);
            else { await UniTask.Yield(timing, token); }
        }
        protected async UniTask BreakPoint(float time, PlayerLoopTiming timing)
        {
            var elipsilonTime = 0.0f;
            while (elipsilonTime < time)
            {
                await BreakPoint(timing);
                await UniTask.WaitForFixedUpdate(destroyCancellationToken);
                elipsilonTime += Time.fixedDeltaTime;
            }
        }
        protected async UniTask BreakPoint(float time, PlayerLoopTiming timing, CancellationToken token)
        {
            var elipsilonTime = 0.0f;
            while (elipsilonTime < time)
            {
                await BreakPoint(timing, token);
                await UniTask.WaitForFixedUpdate(token);
                elipsilonTime += Time.fixedDeltaTime;
            }
        }

    }
}