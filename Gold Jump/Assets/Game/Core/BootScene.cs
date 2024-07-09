using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "_Scene/Boot", fileName = "BootScene", order = 0)]
    public class BootScene : Scene
    {
        public SceneType type = SceneType.Boot;
    }
}