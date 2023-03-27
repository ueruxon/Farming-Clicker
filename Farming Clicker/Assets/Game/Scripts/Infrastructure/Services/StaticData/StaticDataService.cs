using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private ProductionArea _productionAreaPrefab;
        
        private Dictionary<string, ProductItemData> _productDataByName;
        private Dictionary<ProductType, ProductItemData> _productDataByType;

        private Dictionary<string, UpgradeItemData> _upgradeDataByName;
        private Dictionary<UpgradeType, UpgradeItemData> _upgradeDataByType;
        private Dictionary<UpgradeGroup, List<UpgradeItemData>> _upgradeGroupsByType;

        private Dictionary<ShopDataType, List<ShopItemData>> _shopDataByType;

        public StaticDataService(IAssetProvider assetProvider)
        {
            Debug.Log("servce");
            _assetProvider = assetProvider;
        }

        public void Init() => 
            LoadData();

        private void LoadData()
        {
            ProductItemData[] productsItemData = _assetProvider.LoadAll<ProductItemData>(AssetPath.ProductsDataPath);
            UpgradeItemData[] upgradesItemData = _assetProvider.LoadAll<UpgradeItemData>(AssetPath.UpgradesDataPath);

            _productDataByName = productsItemData.ToDictionary(x => x.Name, x => x);
            _productDataByType = productsItemData.ToDictionary(x => x.ProductType, x => x);

            _upgradeGroupsByType = _assetProvider.LoadAll<UpgradeItemGroupData>(AssetPath.UpgradesGroupsDataPath)
                .ToDictionary(x => x.UpgradeGroup, x => x.UpgradeItemsData);
            _upgradeDataByType = upgradesItemData
                .ToDictionary(x => x.UpgradeType, x => x);
            _upgradeDataByName = upgradesItemData.ToDictionary(x => x.Name, x => x);
            
            _shopDataByType = new()
            {
                [ShopDataType.Product] = _assetProvider.LoadAll<ShopItemData>(AssetPath.ProductsDataPath)
                    .ToList(),
                [ShopDataType.Upgrade] = _assetProvider.LoadAll<ShopItemData>(AssetPath.UpgradesDataPath)
                    .ToList(),
            };
        }

        public ProductItemData GetDataForProduct(string productName)
        {
            return _productDataByName.TryGetValue(productName, out ProductItemData data)
                ? data
                : null;
        }

        public ProductItemData GetDataForProduct(ProductType type) =>
            _productDataByType.TryGetValue(type, out ProductItemData data)
                ? data
                : null;
        
        public List<ShopItemData> GetDataForShop(ShopDataType type) => 
            _shopDataByType.TryGetValue(type, out List<ShopItemData> data)
            ? data
            : null;

        public Dictionary<UpgradeGroup, List<UpgradeItemData>> GetUpgradesByGroup() => 
            _upgradeGroupsByType;

        public UpgradeItemData GetDataForUpgrade(UpgradeType type) =>
            _upgradeDataByType.TryGetValue(type, out UpgradeItemData data)
                ? data
                : null;
        
        public UpgradeItemData GetDataForUpgrade(string upgradeName) => 
            _upgradeDataByName.TryGetValue(upgradeName, out UpgradeItemData data)
            ? data
            : null;
    }
}