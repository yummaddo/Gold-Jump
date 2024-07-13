using System;
using System.Collections.Generic;
using Game.GamePlay.Boosts;
using UnityEngine;

namespace Game
{
    public class Payment: MonoBehaviour
    {
        private static readonly string _coinPrefsKeyStorage = "CoinKeyStoreCount";
        private ApplicationContext _application;
        public event Action OnPaymentFailed;    
        public event Action<AbstractBoost> OnPaymentBoostFailed;    
        public event Action<AbstractBoost> OnBoostUseFailed;
        public event Action<AbstractBoost> OnBoostUseConfirm;

        public event Action OnCoinPaymentConfirm;
        public event Action OnCoinPaymentFailed;
        public event Action<AbstractBoost,int> OnBoostUpdate;
        public event Action<int> OnCoinUpdate;
        public event Action<AbstractBoost>  OnBoostPaymentConfirm;
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
            _application = gameObject.GetComponent<ApplicationContext>();
        }
        internal void TryToPayBoost(int value, AbstractBoost boost)
        {
            if (HasConcreteAmountOfCoin(value))
            {
                SetAmountOfBoost(value, boost);
                OnBoostPaymentConfirm?.Invoke(boost);
                OnBoostUpdate?.Invoke(boost,value);
            }
            else
            {
                OnPaymentBoostFailed?.Invoke(boost);
            }
        }
        internal void TryToPayCoins(int value)
        {
            if (HasConcreteAmountOfCoin(value))
            {
                AddCoin(-value);
                OnCoinPaymentConfirm?.Invoke();
            }
            else
            {
                OnPaymentFailed?.Invoke();
            }
        }
        
        public int GetAmountOfCoin() => PlayerPrefs.GetInt(_coinPrefsKeyStorage);
        public int GetAmountOfBoost(AbstractBoost boost) => PlayerPrefs.GetInt(boost.BoostPrefsTitle);

        private void SetAmountOfCoin(int value)
        {
            PlayerPrefs.SetInt(_coinPrefsKeyStorage, value);
            OnCoinUpdate?.Invoke(value);
        }

        private void SetAmountOfBoost(int value, AbstractBoost boost)
        {
            PlayerPrefs.SetInt(boost.BoostPrefsTitle, value);
            OnBoostUpdate?.Invoke(boost,value);
        }
        internal void AddCoin(int value=0)
        {
            var result= PlayerPrefs.GetInt(_coinPrefsKeyStorage) + value;
            SetAmountOfCoin(result);
        }
        internal void AddBoost( AbstractBoost boost, int value=1)
        {
            var result = PlayerPrefs.GetInt(boost.BoostPrefsTitle) + value;
            SetAmountOfBoost(result,boost);
        }
        internal void TryUseBoost( AbstractBoost boost, int value=1)
        {
            var result = PlayerPrefs.GetInt(boost.BoostPrefsTitle) - value;
            if (result < 0)
                OnBoostUseFailed?.Invoke(boost);
            else
            {
                SetAmountOfBoost(result,boost);
                OnBoostUseConfirm?.Invoke(boost);
            }
        }
        public bool HasConcreteAmountOfCoin(int value)
        {
            if (!PlayerPrefs.HasKey(_coinPrefsKeyStorage)) PlayerPrefs.SetInt(_coinPrefsKeyStorage, 0);
            return PlayerPrefs.GetInt(_coinPrefsKeyStorage) > value;
        }
        public bool HasConcreteAmountOfBoost(AbstractBoost boost)
        {
            if (!PlayerPrefs.HasKey(boost.BoostPrefsTitle)) PlayerPrefs.SetInt(boost.BoostPrefsTitle, 0);
            return PlayerPrefs.GetInt(boost.BoostPrefsTitle) > 0;
        }

        public void ResetAll()
        {
            SetAmountOfCoin(0);
        }
    }
}