using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Tutorials;
using Game.Scripts.Logic.Upgrades;
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
        private readonly UpgradesHandler _upgradesHandler;
        private readonly TutorialController _tutorialController;

        public GameInitializer(GameConfig gameConfig,
            IStaticDataService staticDataService,
            IGameProgressService gameProgressService,
            UIFactory uiFactory,
            FarmController farmController,
            UpgradesHandler upgradesHandler, 
            TutorialController tutorialController)
        {
            _gameConfig = gameConfig;
            _staticDataService = staticDataService;
            _gameProgressService = gameProgressService;
            _uiFactory = uiFactory;
            _farmController = farmController;
            _upgradesHandler = upgradesHandler;
            _tutorialController = tutorialController;

            LoadProgressOrInitNew();
            InitializeSystems();
            InitUI();
            InitGameWorld();
            InitTutorial();
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

        private void InitializeSystems()
        {
            _staticDataService.Init();
            _upgradesHandler.Init();
            _farmController.Init();
        }

        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateHUD();
            _uiFactory.CreateShop();
            _uiFactory.CreateSelectProductArea();
            _uiFactory.CreateTutorialWindow();
        }

        private void InitGameWorld() => 
            _farmController.CreateFarm();

        private void InitTutorial() => 
            _tutorialController.Init(_uiFactory);
    }
}