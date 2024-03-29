﻿using System;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Windows.HUD.Elements;
using Game.Scripts.UI.Windows.SelectArea;
using Game.Scripts.UI.Windows.Shop;
using Game.Scripts.UI.Windows.Shop.Elements;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Services.Factory
{
    public class UIFactory
    {
        public event Action ShopOpened;
        
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameProgressService _gameProgressService;
        private readonly FarmController _farmController;
        private readonly UpgradesHandler _upgradesHandler;
        private readonly IInstantiator _instantiator;

        private Transform _uiRoot;
        private OpenShopButton _shopButton;

        public UIFactory(IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            IGameProgressService gameProgressService,
            FarmController farmController,
            UpgradesHandler upgradesHandler,
            IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _farmController = farmController;
            _upgradesHandler = upgradesHandler;
            _instantiator = instantiator;
            _gameProgressService = gameProgressService;
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
            _shopButton.ShopOpened += OnShopOpened;
        }

        public ShopItem CreateShopItem(ShopItemData shopItemData, Transform parent)
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

        public void CreateTutorialWindow() => 
            _instantiator.InstantiatePrefabResource(AssetPath.UITutorialWindowPath, _uiRoot);

        private void OnShopOpened() => 
            ShopOpened?.Invoke();

        public void CleanUp() => 
            _shopButton.ShopOpened -= OnShopOpened;
    }
}