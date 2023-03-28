using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Infrastructure.Setup.Game;
using Zenject;


namespace Game.Scripts.Infrastructure.Setup.Installers
{
    public class ServicesInstaller : Installer<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle().NonLazy();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle().NonLazy();
            Container.Bind<IGameProgressService>().To<GameProgressService>().AsSingle().NonLazy();
            Container.Bind<GameFactory>().AsSingle().NonLazy();
            
            Container.Bind<GameInitializer>().AsSingle();
            Container.Bind<SceneInitializer>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}