using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Boot
{
    public class ApplicationBoot : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ApplicationContext>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<Payment>().FromComponentOn(gameObject).AsSingle();
        }
    }
}