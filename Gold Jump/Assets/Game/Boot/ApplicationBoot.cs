using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Boot
{
    public class ApplicationBoot : MonoInstaller
    {
        public GameObject applicationContext;
        public override void InstallBindings()
        {
            Container.Bind<Application>().FromComponentOn(applicationContext).AsSingle();
        }
    }
}