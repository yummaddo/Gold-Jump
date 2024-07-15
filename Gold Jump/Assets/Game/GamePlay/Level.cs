using System;
using Game.GamePlay.Utility;
using UnityEngine;

namespace Game.GamePlay
{

    [Serializable]
    public class GameObjectPlatformListDictionary : UnitySerializedDictionary<Color, GameObject> { }
    [Serializable]
    public class Level
    {
        public int height = 10000;
        public int preGenerateSize = 4000;
        public float panelsIteratorSize = 2;
        public float iterationBaseSize = 10;
        
        public AnimationCurve chanceOfWidth = AnimationCurve.Linear(0,0,1,1);
        public AnimationCurve spawnPlatformChance;
        public AnimationCurve spawnPlatformNextIterationChance;
        
        public Gradient dispersionOfBasePlatformType;
        public GameObjectPlatformListDictionary platformListDictionary;
    }
}