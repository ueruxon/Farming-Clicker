using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Logic.Production;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        public void Init();
        public ProductData GetDataForProduct(ProductType type);
    }
}