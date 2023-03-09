using Game.Scripts.Data;
using Game.Scripts.Logic.GridLayout;

namespace Game.Scripts.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly GridSystem _gridSystem;

        public GameInitializer(GameConfig gameConfig, GridSystem gridSystem)
        {
            _gameConfig = gameConfig;
            _gridSystem = gridSystem;
            
            InitGameWorld();
        }

        private void InitGameWorld()
        {
            _gridSystem.CreateGrid();
        }
    }
}