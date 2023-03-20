using System.Collections.Generic;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Logic.Production;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        public void Init();
        public ProductItemData GetDataForProduct(ProductType type);
        public ProductItemData GetDataForProduct(string productName);
        public List<ShopItemData> GetDataForShop(ShopDataType type);
        Dictionary<UpgradeGroup, List<UpgradeItemData>> GetUpgradesByGroup();
        public UpgradeItemData GetDataForUpgrade(UpgradeType type);
        public UpgradeItemData GetDataForUpgrade(string upgradeName);
    }
}