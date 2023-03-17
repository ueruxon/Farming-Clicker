using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.UI.Services.Factory;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameProgressService _gameProgressService;
        private readonly UIFactory _uiFactory;
        private readonly FarmController _farmController;

        public GameInitializer(GameConfig gameConfig,
            IStaticDataService staticDataService,
            IGameProgressService gameProgressService,
            UIFactory uiFactory,
            FarmController farmController)
        {
            _gameConfig = gameConfig;
            _staticDataService = staticDataService;
            _gameProgressService = gameProgressService;
            _uiFactory = uiFactory;
            _farmController = farmController;

            LoadProgressOrInitNew();
            InitializeSystems();
            InitUI();
            InitGameWorld();
        }
        
        // async
        private void LoadProgressOrInitNew() => 
            _gameProgressService.Progress = NewProgress();

        private GameProgress NewProgress()
        {
            GameProgress progress = new GameProgress();

            foreach (ResourceData resourceData in _gameConfig.GetInitialResources())
                progress.ResourceRepository.AddResource(resourceData.ResourceType, resourceData.Amount);

            return progress;
        }

        private void InitializeSystems() => 
            _staticDataService.Init();

        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateHUD();
            _uiFactory.CreateShop();
            _uiFactory.CreateSelectProductArea();
        }

        private void InitGameWorld() => 
            _farmController.InitFarm();
    }
}