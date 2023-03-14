using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.UI.Services.Factory;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly IStaticDataService _staticDataService;
        private readonly UIFactory _uiFactory;
        private readonly FarmController _farmController;

        public GameInitializer(GameConfig gameConfig,
            IStaticDataService staticDataService,
            UIFactory uiFactory,
            FarmController farmController)
        {
            _gameConfig = gameConfig;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _farmController = farmController;

            InitializeSystems();
            InitUI();
            InitGameWorld();
        }

        private void InitializeSystems()
        {
            _staticDataService.Init();
        }

        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateHUD();
            _uiFactory.CreateShop();
        }

        private void InitGameWorld()
        {
            _farmController.InitFarm();
        }
    }
}