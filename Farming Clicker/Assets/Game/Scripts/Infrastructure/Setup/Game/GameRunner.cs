using Game.Scripts.Infrastructure.Setup;
using Game.Scripts.Infrastructure.Setup.Game;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameRunner : MonoBehaviour
    {
        private const string GameSceneName = "GameScene";
        
        private GameInitializer _gameInitializer;
        private SceneLoader _sceneLoader;

        [Inject]
        public void InjectDependencies(GameInitializer gameInitializer, SceneLoader sceneLoader)
        {
            _gameInitializer = gameInitializer;
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            InitializeSystems();
            LoadGameProgress();
            LoadNextScene();
        }

        private void InitializeSystems() => 
            _gameInitializer.InitializeSystems();

        private void LoadGameProgress() =>
            _gameInitializer.LoadGameProgress();

        private void LoadNextScene()
        {
#pragma warning disable 4014
            _sceneLoader.Load(GameSceneName);
#pragma warning disable 4014
        }
    }
}
