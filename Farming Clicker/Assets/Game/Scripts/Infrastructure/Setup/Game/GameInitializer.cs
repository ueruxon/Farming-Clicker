using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;

namespace Game.Scripts.Infrastructure.Setup.Game
{
    public class GameInitializer
    {
        private readonly IGameProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly GameConfig _gameConfig;

        public GameInitializer(IGameProgressService progressService, 
            IStaticDataService staticDataService, GameConfig gameConfig)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _gameConfig = gameConfig;
        }

        public void InitializeSystems() => 
            _staticDataService.Init();

        public void LoadGameProgress() => 
            LoadProgressOrInitNew();

        // async?
        private void LoadProgressOrInitNew() => 
            _progressService.Progress = NewProgress();
        
        private GameProgress NewProgress()
        {
            GameProgress progress = new GameProgress();

            foreach (ResourceData resourceData in _gameConfig.GetInitialResources())
                progress.ResourceRepository.AddResource(resourceData.ResourceType, resourceData.Amount);

            return progress;
        }
    }
}