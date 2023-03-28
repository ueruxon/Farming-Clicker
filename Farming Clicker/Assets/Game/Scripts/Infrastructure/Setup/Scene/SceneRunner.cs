using Game.Scripts.Common;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.Setup.Game
{
    public class SceneRunner : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Transform _gridContainer;

        private SceneInitializer _sceneInitializer;
        
        [Inject]
        public void InjectDependencies(SceneInitializer sceneInitializer) => 
            _sceneInitializer = sceneInitializer;

        private void Awake() => 
            _sceneInitializer.SetupScene(_gridContainer);

        private void OnDestroy() => 
            _sceneInitializer.CleanUp();
    }
}