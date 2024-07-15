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
        private const string LastLoginDateStatusKey = "LastLoginDateStatusKey";

        private void Start()
        {
#if !UNITY_EDITOR
            isEditor = false;
#endif
            CheckDailyLogin();
        }
        public void CheckDailyLogin()
        {
            string lastLoginDate = PlayerPrefs.GetString(LastLoginDateKey, "");
            bool lastLoginDateStatus= 9 == PlayerPrefs.GetInt(LastLoginDateStatusKey, 0);
            DateTime currentDate = DateTime.Now.Date;
            DateTime savedDate;
            if (DateTime.TryParse(lastLoginDate, out savedDate))
            {
                if (savedDate < currentDate || !lastLoginDateStatus)
                {
                    isEditor = false;
                    control.OpenDaily();
                    PlayerPrefs.SetString(LastLoginDateKey, currentDate.ToString("yyyy-MM-dd"));
                    PlayerPrefs.SetInt(LastLoginDateStatusKey, 9);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                PlayerPrefs.SetString(LastLoginDateKey, currentDate.ToString("yyyy-MM-dd"));
                PlayerPrefs.Save();
            }
        }
    }
}