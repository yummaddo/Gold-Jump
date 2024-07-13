using System;
using Game.Core;
using Game.UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Game
{
    public class MenuSession : MonoBehaviour
    {
        [Inject] private ApplicationContext _application;
        [Inject] private Payment _payment;
        [SerializeField] private bool isEditor = false; 
        public MenuControl control;
        public void TryToLoad(LevelScene scene)
        {
            _application.LoadConcrete(scene);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
        public void ResetAll()
        {
            _payment.ResetAll();
        }

        private const string LastLoginDateKey = "LastLoginDate";
        private void Start()
        {
#if !UNITY_EDITOR
            isEditor = false;
#endif
            CheckDailyLogin();
        }
        private void CheckDailyLogin()
        {
            string lastLoginDate = PlayerPrefs.GetString(LastLoginDateKey, "");
            DateTime currentDate = DateTime.Now.Date;
            DateTime savedDate;
            if (DateTime.TryParse(lastLoginDate, out savedDate))
            {
                if (savedDate < currentDate || isEditor)
                {
                    isEditor = false;
                    control.OpenDaily();
                }
            }
            PlayerPrefs.SetString(LastLoginDateKey, currentDate.ToString("yyyy-MM-dd"));
            PlayerPrefs.Save();
        }
    }
}