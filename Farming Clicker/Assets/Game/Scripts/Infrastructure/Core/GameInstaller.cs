using Game.Scripts.Common;
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
        
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;
        private IGameProgressService _progressService;
        private GameFactory _gameFactory;
        private UIFactory _uiFactory;

        private GridSystem _gridSystem;
        private FarmController _farmController;
        private CameraController _cameraController;
        private UpgradesHandler _upgradesHandler;
        
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
            _assetProvider = new AssetProvider();
            _staticDataService = new StaticDataService(_assetProvider);
            _progressService = new GameProgressService();
            _gameFactory = new GameFactory(_assetProvider, _staticDataService, _progressService, _gameConfig);

            _gridSystem = new GridSystem(_gameConfig.Width, _gameConfig.Height, 
                _gameConfig.CellSize, _gameConfig.CellPrefab, _gridContainer, _gameConfig.OpenCellByDefault);
            _farmController = new FarmController(_progressService, _staticDataService, _gameFactory, _gridSystem);
            _cameraController = new CameraController(_coroutineRunner, _farmController);
            _upgradesHandler = new UpgradesHandler(_staticDataService, _progressService);

            _uiFactory = new UIFactory(_assetProvider, _staticDataService, _progressService, _farmController, _upgradesHandler);

            _gameInitializer = new GameInitializer(_gameConfig, 
                _staticDataService,
                _progressService,
                _uiFactory,
                _farmController,
                _upgradesHandler);
        }

        public void Cleanup()
        {
            _farmController.Cleanup();
            _cameraController.Cleanup();
        }
    }
}