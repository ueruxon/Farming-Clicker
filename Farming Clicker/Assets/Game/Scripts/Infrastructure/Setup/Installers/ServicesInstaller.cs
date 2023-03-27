using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup.Installers
{
    public class ServicesInstaller : Installer<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log("InstallBindings");
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IGameProgressService>().To<GameProgressService>().AsSingle();
            Container.Bind<GameFactory>().AsSingle();
            Debug.Log("InstallBindings 2");
        }
    }
}