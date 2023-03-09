using Game.Scripts.Data;
using Game.Scripts.Logic.GridLayout;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameInstaller
    {
        private readonly GameConfig _gameConfig;
        private readonly Transform _gridContainer;

        private GameInitializer _gameInitializer;
        
        public GameInstaller(GameConfig gameConfig, Transform gridContainer)
        {
            _gameConfig = gameConfig;
            _gridContainer = gridContainer;

            InstallSystems();
        }

        private void InstallSystems()
        {
            GridSystem gridSystem = new GridSystem(_gameConfig.Width, _gameConfig.Height, 
                _gameConfig.CellSize, _gameConfig.CellPrefab, _gridContainer);


            _gameInitializer = new GameInitializer(_gameConfig, gridSystem);
        }
    }
}