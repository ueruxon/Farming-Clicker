using Game.Scripts.Common;
using Game.Scripts.Data.Game;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameRunner : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Transform _gridContainer;
        
        private GameInstaller _gameInstaller;
        
        private void Awake()
        {
            _gameInstaller = new GameInstaller(_gameConfig, _gridContainer, this);
        }

        private void OnDestroy() => 
            _gameInstaller.Cleanup();
    }
}
