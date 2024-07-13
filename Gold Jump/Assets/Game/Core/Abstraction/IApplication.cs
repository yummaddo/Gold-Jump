using System;

namespace Game.Core.Abstraction
{
    public interface IApplication
    {
        public Action<float> OnLoadingScene { get; set; }
        public Action OnSceneLoad { get; set; }
        public Action OnLevelLoadAwaitStart { get; set; }
        public Action OnLevelLoadAwaitEnd { get; set; }
        public Action OnGameEnd { get; set; }
        public Action OnSessionFadeOff { get; set; }
        public float StaticLoadTime { get; set; }
    }
}