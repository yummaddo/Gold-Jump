using System.Security.Cryptography;
using UnityEngine;

namespace Game.GamePlay.Boosts
{
    public abstract class AbstractBoost : ScriptableObject, IBoost
    {
        [field: SerializeField] public string BoostPrefsTitle { get; set; } = "";
        [field: SerializeField] public int Price { get; set; } = 0;
        public abstract void Activate();
        public void PaymentProvide() => Payment.Instance.TryToPayCoins(Price);
        protected bool HasAmount() => Payment.Instance.HasConcreteAmountOfBoost(this);
    }
}