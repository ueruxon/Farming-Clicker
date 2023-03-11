using Game.Scripts.Data;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Factory
{
    public class GameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly GameConfig _gameConfig;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticDataService, GameConfig gameConfig)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _gameConfig = gameConfig;
        }

        public ProductionArea CreateProductionArea(ProductType productType, Vector3 at)
        {
            ProductData productData = _staticDataService.GetDataForProduct(productType);
            ProductionArea productionArea = _assetProvider.Instantiate<ProductionArea>(AssetPath.ProductAreaPath, at);
            productionArea.Init(productData, _gameConfig.CellSize);

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