using Game.GamePlay.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GamePlay.MatrixMap
{
    [System.Serializable]
    public class MapDailyTemplate : UnitySerializedDictionary<int, GameObject> { }
    
    [CreateAssetMenu(menuName = "MapDailyTemplate", fileName = "MapDailyTemplate", order = 0)]
    public class MapTemplate : ScriptableObject
    {
        public Sprite targetImage;
        public string nameOfMap;
        public MapDailyTemplate template;
    }
}