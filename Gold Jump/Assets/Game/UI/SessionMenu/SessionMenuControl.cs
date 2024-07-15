using System;
using Game.GamePlay;
using Game.GamePlay.Services;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.UI.SessionMenu
{
    public class SessionMenuControl : MonoBehaviour
    {
        private int _countOfRelife = 0;
        
        [Inject] private Session _session;
        [Inject] private Payment _payment;
        [Inject] private Player _player;
        [Inject] private GameMapService _serviceMap;
        [Inject] private TimerService _timerService;
        [Inject] private ApplicationContext _application;
        
        public GameObject deadPanelWithTimer;
        public GameObject wonPanelWithStars;
        public GameObject notAmountOfCoin;

        public HealthView healthView;
        
        public StartControl starFirst;
        public StartControl starSecond;
        public StartControl starThird;
        public TextMeshProUGUI coinTextMeshProUGUI;
        private void Awake()
        {
            _session.OnStopSession += OnStopSession;
            _session.OnReStartSession += OnReStartSession;
            _session.OnScenePlayerDead += OnScenePlayerDead;
            _session.OnScenePlayerWon += OnScenePlayerWon;
        }
        public void ByReLifeGold() => healthView.ExtraLifeByAndUse();
        public void ByReLifeItem() => healthView.ExtraLifeUse();
        public void StartOver() => ApplicationContext.Instance.ImmediatelyMenu();
        public void ViewCoinFailed() => notAmountOfCoin.SetActive(true);

        private void OnScenePlayerWon()
        {
            try
            {
                wonPanelWithStars.SetActive(true);
                starFirst.Activate();
                var intValue = 1;
                var level = _serviceMap.levelScene;
                var currentProgress = PlayerPrefs.GetInt(level.levelPefsName);
                var coins = Random.Range(level.firstStarCoinsRange.x, level.firstStarCoinsRange.y);
                if (_timerService.totalTime < level.timeToStar)
                {
                    intValue++;
                    coins += Random.Range(level.secondStarCoinsRange.x, level.secondStarCoinsRange.y);
                    starSecond.Activate();
                }
                else starSecond.DeActivate();

                if (_serviceMap.KilledAll())
                {
                    intValue++;
                    coins += Random.Range(level.thirdStarCoinsRange.x, level.thirdStarCoinsRange.y);
                    starThird.Activate();
                }
                else
                {
                    starThird.DeActivate();
                }
                if (currentProgress< intValue)  PlayerPrefs.SetInt(level.levelPefsName, intValue);
                coinTextMeshProUGUI.text = coins.ToString();
                _payment.AddCoin(coins);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        private void OnScenePlayerDead()
        {
            try
            {
                var intValue = 0;
                var level = _serviceMap.levelScene;
                var currentProgress = PlayerPrefs.GetInt(level.levelPefsName);
                deadPanelWithTimer.SetActive(true);
                if (currentProgress< intValue)  PlayerPrefs.SetInt(level.levelPefsName, intValue);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        private void OnReStartSession() { }
        private void OnStopSession() { }
        public void ReLife()
        {
            try
            {
                _session.ScenePlayerReLife();
                deadPanelWithTimer.SetActive(false);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}