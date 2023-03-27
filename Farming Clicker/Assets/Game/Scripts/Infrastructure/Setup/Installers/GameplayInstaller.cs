using Game.Scripts.Logic;
using Game.Scripts.Logic.Cameras;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Tutorials;
using Game.Scripts.Logic.Upgrades;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup.Installers
{
    public class GameplayInstaller : Installer<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GridSystem>().AsSingle();
            Container.Bind<FarmController>().AsSingle();
            Container.Bind<CameraController>().AsSingle();
            Container.Bind<UpgradesHandler>().AsSingle();
            Container.Bind<TutorialController>().AsSingle();
        }
    }
}