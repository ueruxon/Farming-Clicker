using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private ProductionArea _productionAreaPrefab;
        private Dictionary<ProductType, ProductData> _productDataByType;

        public StaticDataService(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public void Init() => 
            LoadData();

        private void LoadData()
        {
            _productDataByType = _assetProvider.LoadAll<ProductData>(AssetPath.ProductsDataPath)
                .ToDictionary(x => x.ProductType, x => x);
        }

        public ProductData GetDataForProduct(ProductType type) =>
            _productDataByType.TryGetValue(type, out ProductData data)
                ? data
                : null;
    }
}