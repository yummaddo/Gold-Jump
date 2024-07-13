using System.Collections.Generic;
using Game.GamePlay.Utility;
using UnityEngine;

namespace Game.UI.MainMenu
{

    public enum MenuType
    {
        Main,Shop,Faq,Info,Quest,Map, Daily, Setting, GetCoin
    }
    [System.Serializable]
    public class Group
    {
        public MenuType menuType;
        public GameObject content;
        public GameObject footer;
        public override bool Equals(object obj)
        {
            return menuType.Equals(obj);
        }
        protected bool Equals(Group other)
        {
            return menuType == other.menuType;
        }
        public override int GetHashCode()
        {
            return (int)menuType;
        }
    }

    [System.Serializable]
    public class MenuGameObjectDictionary : UnitySerializedDictionary<string, Group> { }
    public class MenuControl : MonoBehaviour
    {
        [SerializeField] public MenuGameObjectDictionary dictionary = new MenuGameObjectDictionary();
        public MenuType current = MenuType.Main;
        public MenuType last = MenuType.Main;
        public void OpenMain() => ActivateNewMenu(MenuType.Main);
        public void OpenShop() => ActivateNewMenu(MenuType.Shop);
        public void OpenFaq() => ActivateNewMenu(MenuType.Faq);
        public void OpenInfo() => ActivateNewMenu(MenuType.Info);
        public void OpenSetting() => ActivateNewMenu(MenuType.Setting);
        public void OpenDaily() => ActivateNewMenu(MenuType.Daily);
        public void OpenQuest() => ActivateNewMenu(MenuType.Quest);
        public void OpenGetCoin() => ActivateNewMenu(MenuType.GetCoin);
        public void OpenMap() => ActivateNewMenu(MenuType.Map);
        public void Confirm()
        {
            
        }
        public void ComeBack()
        {
            if (last == current) return;
            SetOff(current);
            (current, last) = (last, current);
            SetOn(current);
        }
        private void ActivateNewMenu(MenuType type)
        {
            if (type == current) return;
            SetOff(current);
            last = current;
            current = type;
            SetOn(current);
        }
        private void SetOff(MenuType yMenuType)
        {
            var obj = dictionary[yMenuType.ToString()];
            obj.content.SetActive(false);
            obj.footer.SetActive(false);
        }
        private void SetOn(MenuType yMenuType)
        {
            var obj = dictionary[yMenuType.ToString()];
            obj.content.SetActive(true);
            obj.footer.SetActive(true);
        }
    }
}