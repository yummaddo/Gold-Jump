using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.GamePlay.Boosts;
using UnityEngine;

namespace Game.GamePlay
{
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();
        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
                this[this.keyData[i]] = this.valueData[i];
        }
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }
    [Serializable]
    public class GameObjectPlatformListDictionary : UnitySerializedDictionary<Color, GameObject> { }
    [Serializable]
    public class Level
    {
        public int height = 10000;
        public int preGenerateSize = 4000;
        
        public int panelsIteratorSize = 2;
        public int iterationBaseSize = 10;
        
        public AnimationCurve chanceOfWidth = AnimationCurve.Linear(0,0,1,1);
        public AnimationCurve spawnPlatformChance;
        public AnimationCurve spawnPlatformNextIterationChance;
        
        public Gradient dispersionOfBasePlatformType;
        public GameObjectPlatformListDictionary platformListDictionary;
    }
}