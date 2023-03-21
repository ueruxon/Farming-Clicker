using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Production;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Windows.HUD.Elements;
using Game.Scripts.UI.Windows.SelectArea;
using Game.Scripts.UI.Windows.Shop;
using Game.Scripts.UI.Windows.Shop.Elements;
using UnityEngine;

namespace Game.Scripts.UI.Services.Factory
{
    public class UIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameProgressService _gameProgressService;
        private readonly FarmController _farmController;
        private readonly UpgradesHandler _upgradesHandler;

        private Transform _uiRoot;

        public UIFactory(IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            IGameProgressService gameProgressService,
            FarmController farmController,
            UpgradesHandler upgradesHandler)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _farmController = farmController;
            _upgradesHandler = upgradesHandler;
            _gameProgressService = gameProgressService;
        }

        private OpenShopButton _shopButton;

        public void CreateUIRoot() => 
            _uiRoot = _assetProvider.Instantiate<GameObject>(AssetPath.UIRootPath).transform;

        public void CreateHUD()
        {
            GameObject hud = _assetProvider.Instantiate<GameObject>(AssetPath.UIHudPath, _uiRoot);
            
            _shopButton = hud.GetComponentInChildren<OpenShopButton>();
            
            CancelAreaButton cancelButton = hud.GetComponentInChildren<CancelAreaButton>();
            cancelButton.Init(_farmController);
            cancelButton.Hide();

            ResourceCounter counter = hud.GetComponentInChildren<ResourceCounter>();
            counter.Init(_gameProgressService);

            InteractionCropsButton interactionButton = hud.GetComponentInChildren<InteractionCropsButton>();
            interactionButton.Init(_gameProgressService, _farmController);
        }

        public void CreateShop()
        {
            ShopWindow shopWindow = _assetProvider.Instantiate<ShopWindow>(AssetPath.UIShopWindowPath, _uiRoot);
            shopWindow.Init(_staticDataService, this, _farmController, _upgradesHandler);
            shopWindow.Close();
            
            _shopButton.Init(shopWindow, _farmController);
        }

        public ShopItem CreateShopItem(ShopItemData shopItemData, Transform parent, ShopDataType shopDataType)
        {
            ShopItem shopItem = _assetProvider.Instantiate<ShopItem>(AssetPath.UIShopItemPath, parent);
            shopItem.Init(_gameProgressService, shopItemData);

            return shopItem;
        }

        public void CreateSelectProductArea()
        {
            SelectAreaWindow areaWindow =
                _assetProvider.Instantiate<SelectAreaWindow>(AssetPath.UISelectAreaPath, _uiRoot);
            areaWindow.Init(_gameProgressService, _farmController);
            areaWindow.Hide();
        }
    }
}