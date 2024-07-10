using UnityEngine;
using Zenject;

namespace Game.Boot
{
    public class MenuBoot : MonoInstaller
    {
        public GameObject menuBoot;
        public override void InstallBindings()
        {
            Container.Bind<MenuBoot>().FromComponentOn(menuBoot).AsSingle();
        }
    }
}