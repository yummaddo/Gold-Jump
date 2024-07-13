using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnitySceneReference;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Application", fileName = "Application", order = 0)]
    public class ApplicationSetting : ScriptableObject
    {
        public int CurrentCoinsCount => currentCoins;
        public bool isWelcome = false;
        [SerializeField] private int currentCoins = 0;
        [FormerlySerializedAs("scenesList")]         public List<LevelScene>  scenesList = new List<LevelScene>();
        [FormerlySerializedAs("mainMenu")]           public SceneReference    mainMenu;
        [FormerlySerializedAs("welcomeScene")]       public SceneReference    welcomeScene;
        [FormerlySerializedAs("currentSceneLevel")]  public LevelScene        currentSceneLevel;
        [FormerlySerializedAs("bootScene")]          public BootScene         bootScene;
        
        internal void LoadNext() => TryTyResetToNext();
        internal void LoadConcrete(LevelScene scene)
        {
            if (scenesList.Contains(scene)) currentSceneLevel = scene;
        } 

        
        #region Context Methods
        [ContextMenu("Reset To First")] private void TryTyResetToFirst()
        {
            var list = scenesList;
            var minId = list.Select((l)=> l.id).Min();
            var level = list.Find((t) => t.id == minId);
            currentSceneLevel = level;
        }
        [ContextMenu("Reset To End")] private void TryTyResetToEnd()
        {
            var list = scenesList;
            var minId = list.Select((l)=> l.id).Max();
            var level = list.Find((t) => t.id == minId);
            currentSceneLevel = level;
        }
        [ContextMenu("Prev level")] private void TryTyResetToPrev()
        {
            var list = scenesList;
            var level = list.Find((t) => t.id == currentSceneLevel.id - 1);
            if (level) currentSceneLevel = level;
            else
            {
                Debug.Log($"<color=#7B00FF>[!Application] Does not find previous level with index [{ currentSceneLevel.id -1}]\n [Load] END</color>");
                TryTyResetToEnd();
            }
        }
        [ContextMenu("Next level")] private void TryTyResetToNext()
        {
            var level = scenesList.Find((t) => t.id == currentSceneLevel.id + 1);
            if (level) currentSceneLevel = level;
            else
            {
                Debug.Log($"<color=#7B00FF>[!Application] Does not find next level with index [{ currentSceneLevel.id +1}]\n [Load] FIRST</color>");
                TryTyResetToFirst();
            }
        }
        [ContextMenu("Random level")] private void TryTyResetToRandom()
        {
            var id = GetRandomValue(scenesList.Select(levelScene => levelScene.id).ToList(), currentSceneLevel.id);
            var levelScene = scenesList.FirstOrDefault(t => t.id == id);
            currentSceneLevel = levelScene;
        }
        private int GetRandomValue(List<int> valuesList, int excludedId)
        {
            List<int> validValues = valuesList.FindAll(value => value != excludedId);
            if (validValues.Count == 0) return excludedId; 
            int randomIndex = Random.Range(0, validValues.Count-1);
            return validValues[randomIndex]; 
        }
        #endregion
    }
}