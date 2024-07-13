using System;
using Game.Core.Types;
using Game.GamePlay;
using UnityEngine;
using UnitySceneReference;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "_Scene/Crate_Level_Template", fileName = "Level_1", order = 0)]
    public class LevelScene : ScriptableObject
    {
        public SceneReference sceneReference;
        public int id = 0;
        public Sprite backGround;
        internal string levelPefsName => $"LevelScene{id}";
        public float timeToStar = 100;
        [Space(10)] [Header("Scene Parameters ")]
        public int maxEnemy = 2;
        public GameObject enemyPrefab;
        [Range(0,1)]public float percentSpawnEnemyByIteration = 0.02f;
        [Range(0, 10000)] public float createHeightMinimal = 200f;
        // public AnimationCurve enemyCountDispersion;
        // public AnimationCurve enemyHeightDispersion;
        public SceneType type = SceneType.Level;
        public Level data;
        public Vector2Int firstStarCoinsRange;
        public Vector2Int secondStarCoinsRange;
        public Vector2Int thirdStarCoinsRange;
    }
}