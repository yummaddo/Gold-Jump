using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Abstraction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GamePlay.Services
{
    public class TimerService : AbstractServices
    {
        public TextMeshProUGUI textMeshProUGUI;
        public float totalTime = 0;
        public override void OnAwake() { }
        public override void OnStart()
        {
            TimerProcedure().Forget();
        }
        private async UniTask TimerProcedure()
        {
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                await BreakPoint();
                await UniTask.WaitForFixedUpdate();
                totalTime += Time.fixedDeltaTime;
                textMeshProUGUI.text = GetFormation();
            }
        }
        private string GetFormation()
        {
            var minutes = (int)totalTime/60;
            var seconds = (int)totalTime%60;
            var minuteTime = minutes < 10 ? $"0{minutes}" : $"{minutes}";
            var secondsTime = seconds < 10 ? $"0{seconds}" : $"{seconds}";
            return minuteTime + ":" + secondsTime;
        }
    }
}