using System;
using Game.GamePlay.Utility;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.GamePlay.Boosts
{
    
    [Serializable]
    public class GameObjectItemListDictionary : UnitySerializedDictionary<Color, GameObject> { }
    public class ItemSpawner : MonoBehaviour
    {
        [Inject] private DiContainer _containerDi;
        [SerializeField] private RectTransform positionOfItem; 
        public GameObjectPlatformListDictionary listDictionary;
        public Gradient dispersionOfBase;
        private void Awake()
        {
            var valueOfPlatform = Random.Range(0,1f);
            var chanceOfPlatformType = dispersionOfBase.Evaluate(valueOfPlatform);
            var itemType = listDictionary[chanceOfPlatformType];
            if (itemType != null)
                _containerDi.InstantiatePrefab(itemType,positionOfItem.transform.parent.parent.parent);
        }
    }
}