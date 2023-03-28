using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.UI;
using Game.Scripts.UI.Services.Factory;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup.Installers
{
    public class UIInstaller : Installer<UIInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadingCurtain>()
                .FromComponentInNewPrefabResource(AssetPath.UILoadingCurtain)
                .AsSingle()
                .NonLazy();

            Container.Bind<UIFactory>().AsSingle();
        }
    }
}