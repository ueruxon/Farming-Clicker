using System.Collections.Generic;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Logic.Production;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        public void Init();
        public ProductItemData GetDataForProduct(ProductType type);
        public ProductType GetProductType(string productName);
        public List<ShopItemData> GetDataForShop(ShopDataType type);
    }
}