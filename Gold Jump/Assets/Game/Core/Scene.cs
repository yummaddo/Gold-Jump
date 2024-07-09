using UnityEngine;
using UnitySceneReference;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "_Scene/Base", fileName = "Scene", order = 0)]
    public class Scene : ScriptableObject
    {
        [Tooltip("Preset")] 
        public SceneReference scene;
        public string sceneName = "scene_1";
    }
}