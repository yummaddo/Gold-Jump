using System;
using UnityEngine;
using UnitySceneReference;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "_Scene/Crate_Level_Template", fileName = "Level_1", order = 0)]
    public class LevelScene : ScriptableObject
    {
        public SceneReference sceneReference;
        public int id = 0;
        [Space(10)]
        [Header("Scene Parameters ")]
        public SceneType type = SceneType.Level;
    }
}