using UnityEngine;
using Zenject;

namespace Game.Boot
{
    public class SessionBoot : MonoInstaller
    {
        public GameObject sessionContext;
        public override void InstallBindings()
        {
            Container.Bind<Session>().FromComponentOn(sessionContext).AsSingle();
        }
    }
}