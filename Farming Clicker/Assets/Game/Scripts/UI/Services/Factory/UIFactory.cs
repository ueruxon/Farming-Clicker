using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Production;
using Game.Scripts.UI.Elements;
using Game.Scripts.UI.Windows.SelectArea;
using Game.Scripts.UI.Windows.Shop;
using UnityEngine;

namespace Game.Scripts.UI.Services.Factory
{
    public class UIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly FarmController _farmController;

        private Transform _uiRoot;
        private OpenShopButton _shopButton;

        public UIFactory(IAssetProvider assetProvider,
            IStaticDataService staticDataService, FarmController farmController)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _farmController = farmController;
        }
        
        public void CreateUIRoot() => 
            _uiRoot = _assetProvider.Instantiate<GameObject>(AssetPath.UIRootPath).transform;

        public void CreateHUD()
        {
            GameObject hud = _assetProvider.Instantiate<GameObject>(AssetPath.UIHudPath, _uiRoot);
            
            _shopButton = hud.GetComponentInChildren<OpenShopButton>();
            
            CancelAreaButton cancelButton = hud.GetComponentInChildren<CancelAreaButton>();
            cancelButton.Init(_farmController);
            cancelButton.Hide();
        }

        public void CreateShop()
        {
            ShopWindow shopWindow = _assetProvider.Instantiate<ShopWindow>(AssetPath.UIShopWindowPath, _uiRoot);
            shopWindow.Init(_staticDataService, this, _farmController);
            shopWindow.Close();
            
            _shopButton.Init(shopWindow, _farmController);
        }

        public ShopItem CreateShopItem(ShopItemData shopItemData, Transform parent, ShopDataType shopDataType)
        {
            ShopItem shopItem = _assetProvider.Instantiate<ShopItem>(AssetPath.UIShopItemPath, parent);
            FarmData farmData = new FarmData();

            if (shopDataType == ShopDataType.Product)
            {
                ProductType productType = _staticDataService.GetProductType(shopItemData.Name);
                farmData.ProductType = productType;
                farmData.DataType = shopDataType;
            }
            
            shopItem.Init(shopItemData, farmData);
            
            return shopItem;
        }

        public void CreateSelectProductArea()
        {
            SelectAreaWindow areaWindow =
                _assetProvider.Instantiate<SelectAreaWindow>(AssetPath.UISelectAreaPath, _uiRoot);
            areaWindow.Init(_farmController);
            areaWindow.Hide();
        }
    }
}