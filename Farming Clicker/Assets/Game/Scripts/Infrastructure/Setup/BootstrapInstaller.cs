using Game.Scripts.Infrastructure.Setup.Installers;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Вызов");
            ServicesInstaller.Install(Container);
            GameplayInstaller.Install(Container);
            UIInstaller.Install(Container);
        }
    }
}