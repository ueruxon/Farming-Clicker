using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Logic.Production;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private ProductionArea _productionAreaPrefab;
        private Dictionary<string, ProductType> _productTypeByName;
        private Dictionary<ProductType, ProductItemData> _productDataByType;

        private Dictionary<ShopDataType, List<ShopItemData>> _shopDataByType;

        public StaticDataService(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public void Init() => 
            LoadData();

        private void LoadData()
        {
            ProductItemData[] productsItemData = _assetProvider.LoadAll<ProductItemData>(AssetPath.ProductsDataPath);

            _productTypeByName = productsItemData.ToDictionary(x => x.Name, x => x.ProductType);
            _productDataByType = productsItemData.ToDictionary(x => x.ProductType, x => x);
            
            _shopDataByType = new()
            {
                [ShopDataType.Product] = _assetProvider.LoadAll<ShopItemData>(AssetPath.ProductsDataPath)
                    .ToList(),
                [ShopDataType.Upgrade] = _assetProvider.LoadAll<ShopItemData>(AssetPath.UpgradesDataPath)
                    .ToList(),
            };
        }

        public ProductType GetProductType(string productName)
        {
            return _productTypeByName.TryGetValue(productName, out ProductType type)
                ? type
                : ProductType.None;
        }

        public ProductItemData GetDataForProduct(ProductType type) =>
            _productDataByType.TryGetValue(type, out ProductItemData data)
                ? data
                : null;
        
        public List<ShopItemData> GetDataForShop(ShopDataType type) => 
            _shopDataByType.TryGetValue(type, out List<ShopItemData> data)
            ? data
            : null;
    }
}