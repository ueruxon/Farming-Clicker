using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Logic
{
    public class FarmController
    {
        private readonly GameFactory _gameFactory;
        private readonly GridSystem _gridSystem;

        public FarmController(GameFactory gameFactory, GridSystem gridSystem)
        {
            _gameFactory = gameFactory;
            _gridSystem = gridSystem;
        }

        public void InitFarm()
        {
            GridCell[,] grid = _gridSystem.CreateGrid();
            
            
            
            // для теста
            CreateProductionArea(ProductType.Wheat, new Vector2Int(0, 0));
            CreateProductionArea(ProductType.Wheat, new Vector2Int(0, 1));
            CreateProductionArea(ProductType.Wheat, new Vector2Int(0, 2));
            
            CreateProductionArea(ProductType.Corn, new Vector2Int(2, 0));
            CreateProductionArea(ProductType.Corn, new Vector2Int(2, 1));
            CreateProductionArea(ProductType.Corn, new Vector2Int(2, 2));
            
            CreateProductionArea(ProductType.Potato, new Vector2Int(4, 0));
            CreateProductionArea(ProductType.Potato, new Vector2Int(4, 1));
            CreateProductionArea(ProductType.Potato, new Vector2Int(4, 2));
        }

        private void CreateProductionArea(ProductType productType, Vector2Int cellPosition)
        {
            GridCell gridCell = _gridSystem.GetGridCell(cellPosition);
            Vector3 spawnPosition = _gridSystem.GetWorldPosition(cellPosition);
            ProductionArea productionArea = _gameFactory.CreateProductionArea(productType, at: spawnPosition);
            gridCell.AddProductionArea(productionArea);
            
            // для теста
            productionArea.ActivateProduction();
        }
    }
}