using Game.Scripts.UI.Services.Factory;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup.Installers
{
    public class UIInstaller : Installer<UIInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UIFactory>().AsSingle();
        }
    }
}