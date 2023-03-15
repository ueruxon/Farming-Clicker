using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.UI.Services.Factory;
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
            IAssetProvider assetProvider = new AssetProvider();
            IStaticDataService staticDataService = new StaticDataService(assetProvider);
            GameFactory gameFactory = new GameFactory(assetProvider, staticDataService, _gameConfig);
            GridSystem gridSystem = new GridSystem(_gameConfig.Width, _gameConfig.Height, 
                _gameConfig.CellSize, _gameConfig.CellPrefab, _gridContainer, _gameConfig.OpenCellByDefault);
            FarmController farmController = new FarmController(gameFactory, gridSystem);
            UIFactory uiFactory = new UIFactory(assetProvider, staticDataService, farmController);

            _gameInitializer = new GameInitializer(_gameConfig, 
                staticDataService,
                uiFactory,
                farmController);
        }
    }
}