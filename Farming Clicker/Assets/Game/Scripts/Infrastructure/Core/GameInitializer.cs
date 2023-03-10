using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.GridLayout;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly IStaticDataService _staticDataService;
        private readonly FarmController _farmController;

        public GameInitializer(GameConfig gameConfig, 
            IStaticDataService staticDataService,
            FarmController farmController)
        {
            _gameConfig = gameConfig;
            _staticDataService = staticDataService;
            _farmController = farmController;

            InitializeSystems();
            InitGameWorld();
        }

        private void InitializeSystems()
        {
            _staticDataService.Init();
        }

        private void InitGameWorld()
        {
            _farmController.InitFarm();
        }
    }
}