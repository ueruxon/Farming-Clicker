using Game.Scripts.Data;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Transform _gridContainer;
        
        private GameInstaller _gameInstaller;
        
        private void Awake()
        {
            _gameInstaller = new GameInstaller(_gameConfig, _gridContainer);
        }
    }
}
