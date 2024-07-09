using Game.GamePlay.Boosts;
using UnityEngine;

namespace Game.UI
{
    public class UButtonBoostActivate : MonoBehaviour
    {
        public AbstractBoost boost;
        public void BoostActivate() => boost.Activate();
    }
}