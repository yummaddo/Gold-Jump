using UnityEngine;
using Zenject;

namespace Game.GamePlay.Boosts
{
    public class ItemUse : MonoBehaviour
    {
        [Inject] private Player _player;
        public AbstractBoost boost;
        public void Use()
        {
            if (_player.GetStatus())
                boost.Activate();
        }
    }
}