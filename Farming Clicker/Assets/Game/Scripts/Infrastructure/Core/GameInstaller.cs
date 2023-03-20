using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Cameras;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameInstaller
    {
        private readonly GameConfig _gameConfig;
        private readonly Transform _gridContainer;
        private readonly ICoroutineRunner _coroutineRunner;

        private GameInitializer _gameInitializer;
        
        public GameInstaller(GameConfig gameConfig, Transform gridContainer, ICoroutineRunner coroutineRunner)
        {
            _gameConfig = gameConfig;
            _gridContainer = gridContainer;
            _coroutineRunner = coroutineRunner;

            InstallSystems();
        }

        private void InstallSystems()
        {
            IAssetProvider assetProvider = new AssetProvider();
            IStaticDataService staticDataService = new StaticDataService(assetProvider);
            IGameProgressService progressService = new GameProgressService();
            GameFactory gameFactory = new GameFactory(assetProvider, staticDataService, progressService, _gameConfig);

            GridSystem gridSystem = new GridSystem(_gameConfig.Width, _gameConfig.Height, 
                _gameConfig.CellSize, _gameConfig.CellPrefab, _gridContainer, _gameConfig.OpenCellByDefault);
            FarmController farmController = new FarmController(progressService, staticDataService, gameFactory, gridSystem);
            UpgradesHandler upgradesHandler = new UpgradesHandler(staticDataService, progressService); 
            CameraController cameraController = new CameraController(_coroutineRunner, farmController);

            UIFactory uiFactory = new UIFactory(assetProvider, staticDataService, progressService, farmController, upgradesHandler);

            _gameInitializer = new GameInitializer(_gameConfig, 
                staticDataService,
                progressService,
                uiFactory,
                farmController,
                upgradesHandler);
        }
    }
}