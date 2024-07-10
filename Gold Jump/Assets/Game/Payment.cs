using System;
using System.Collections.Generic;
using Game.GamePlay.Boosts;
using UnityEngine;

namespace Game
{
    public class Payment: MonoBehaviour
    {
        private static readonly string _coinPrefsKeyStorage = "CoinKeyStoreCount";
        private Application _application;
        public Action OnPaymentFailed;
        public Action OnCoinPaymentConfirm;
        public Action<AbstractBoost>  OnBoostPaymentConfirm;
        private static Payment _instance;
        internal static Payment Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<Payment>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
            _application = gameObject.GetComponent<Application>();
        }
        internal void TryToPayBoost(int value, AbstractBoost boost)
        {
            if (HasConcreteAmountOfCoin(value))
            {
                SetAmountOfBoost(value, boost);
                OnBoostPaymentConfirm?.Invoke(boost);
            }
            else
            {
                OnPaymentFailed?.Invoke();
            }
        }
        internal void TryToPayCoins(int value)
        {
            if (HasConcreteAmountOfCoin(value))
            {
                SetAmountOfCoin(value);
                OnCoinPaymentConfirm?.Invoke();
            }
            else
            {
                OnPaymentFailed?.Invoke();
            }
        }
        
        public int GetAmountOfCoin() => PlayerPrefs.GetInt(_coinPrefsKeyStorage);
        public int GetAmountOfBoost(AbstractBoost boost) => PlayerPrefs.GetInt(boost.BoostPrefsTitle);

        private void SetAmountOfCoin(int value) => PlayerPrefs.SetInt(_coinPrefsKeyStorage, value);
        private void SetAmountOfBoost(int value, AbstractBoost boost) => PlayerPrefs.SetInt(boost.BoostPrefsTitle, value);
        
        public bool HasConcreteAmountOfCoin(int value)
        {
            if (!PlayerPrefs.HasKey(_coinPrefsKeyStorage))
                PlayerPrefs.SetInt(_coinPrefsKeyStorage, 0);
            return PlayerPrefs.GetInt(_coinPrefsKeyStorage) > 0;
        }
        public bool HasConcreteAmountOfBoost(AbstractBoost boost)
        {
            if (!PlayerPrefs.HasKey(boost.BoostPrefsTitle))
                PlayerPrefs.SetInt(boost.BoostPrefsTitle, 0);
            return PlayerPrefs.GetInt(boost.BoostPrefsTitle) > 0;
        }
    }
}