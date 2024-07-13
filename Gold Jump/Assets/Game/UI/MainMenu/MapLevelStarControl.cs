using System;
using Game.Core;
using Game.UI.SessionMenu;
using UnityEngine;

namespace Game.UI.MainMenu
{
    public class MapLevelStarControl : MonoBehaviour
    {
        public LevelScene scene;
        public StartControl first;
        public StartControl second;
        public StartControl third;
        private void OnEnable()
        {
            var currentProgress = PlayerPrefs.GetInt(scene.levelPefsName);
            Debug.Log($"Progress [{scene.id}]: {currentProgress}");
            if (currentProgress >= 1) first.Activate();
            if (currentProgress >= 2) second.Activate();
            if (currentProgress >= 3) third.Activate();
        }
    }
    
}