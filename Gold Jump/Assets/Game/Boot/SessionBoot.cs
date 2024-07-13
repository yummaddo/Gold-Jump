using Game.GamePlay;
using Game.GamePlay.Services;
using UnityEngine;
using Zenject;

namespace Game.Boot
{
    public class SessionBoot : MonoInstaller
    {
        public GameObject sessionContext;
        public GameObject playerService;
        public GameObject gameMapService;
        public GameObject controllerService;
        public GameObject player;
        public GameObject timer;
        public override void InstallBindings()
        {
            
            Container.Bind<Session>().FromComponentOn(sessionContext).AsSingle();
            Container.Bind<PlayerService>().FromComponentOn(playerService).AsSingle();
            Container.Bind<GameMapService>().FromComponentOn(gameMapService).AsSingle();
            Container.Bind<ControllerService>().FromComponentOn(controllerService).AsSingle();
            Container.Bind<Player>().FromComponentOn(player).AsSingle();
            Container.Bind<TimerService>().FromComponentOn(timer).AsSingle();
        }
    }
}