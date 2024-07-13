using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.UI.MainMenu
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private MenuSession session;
        public LevelScene scene;
        public bool active = false;
        public void LoadLevel()
        {
            if (active) session.TryToLoad(scene);
        }
    }
}