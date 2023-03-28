using Game.Scripts.Common;
using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Setup.Installers;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner, IInitializable
    {
        [SerializeField] private GameConfig _config;
        
        public override void InstallBindings()
        {
            BindGameConfig();
            BindInstallerByInterface();
            
            ServicesInstaller.Install(Container);
            UIInstaller.Install(Container);
            GameplayInstaller.Install(Container);
        }

        private void BindGameConfig() => 
            Container.Bind<GameConfig>().FromInstance(_config).AsSingle();

        private void BindInstallerByInterface()
        {
            Container
                .Bind<ICoroutineRunner>()
                .To<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle()
                .NonLazy();
        }

        public void Initialize()
        {
            Debug.Log("f sda");
            ICoroutineRunner test = Container.Resolve<ICoroutineRunner>();
            
            Debug.Log(test);
        }
    }
}