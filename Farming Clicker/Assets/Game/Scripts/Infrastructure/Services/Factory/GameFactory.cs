using Game.Scripts.Data.Game;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameProgressService _progressService;
        private readonly GameConfig _gameConfig;
        
        public GameFactory(IAssetProvider assetProvider, 
            IStaticDataService staticDataService, 
            IGameProgressService progressService, 
            GameConfig gameConfig)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _progressService = progressService;
            _gameConfig = gameConfig;
        }

        public ProductionArea CreateProductionArea(ProductType productType, Vector3 at)
        {
            ProductItemData productItemData = _staticDataService.GetDataForProduct(productType);
            ProductionArea productionArea = _assetProvider.Instantiate<ProductionArea>(AssetPath.ProductAreaPath, at);
            productionArea.Init(_progressService, productItemData, _gameConfig.CellSize);

            return productionArea;
        }

        public ProductionAreaGhost CreateAreaGhost(Vector3 at)
        {
            var area = _assetProvider.Instantiate<ProductionAreaGhost>(AssetPath.ProductAreaGhostPath, at);
            area.Init(_gameConfig.CellSize);
            
            return area;
        }
    }
}