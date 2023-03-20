using System;
using System.Collections.Generic;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Services.Factory;
using Game.Scripts.UI.Windows.Shop.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.Shop
{
    public class ShopWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space(2)]
        [SerializeField] private TabGroup _tabGroup;

        private IStaticDataService _staticDataService;
        private FarmController _farmController;
        private UpgradesHandler _upgradesHandler;
        
        public void Init(IStaticDataService staticDataService, 
            UIFactory factory, 
            FarmController farmController, 
            UpgradesHandler upgradesHandler)
        {
            _staticDataService = staticDataService;
            _farmController = farmController;
            _upgradesHandler = upgradesHandler;
            
            _tabGroup.Init(staticDataService, factory, upgradesHandler);
            _tabGroup.ItemSelected += OnItemSelected;
            _closeButton.onClick.AddListener(CloseShop);
        }

        public void Open() => _canvasGroup.SetActive(true);
        public void Close() => _canvasGroup.SetActive(false);

        private void OnItemSelected(TabContentType contentType, ShopItemData shopItemData)
        {
            if (contentType == TabContentType.Products)
            {
                var data = _staticDataService.GetDataForProduct(shopItemData.Name);
                _farmController.BuildProductionArea(data.ProductType);
                Close();
            }

            if (contentType == TabContentType.Upgrades)
            {
                var data = _staticDataService.GetDataForUpgrade(shopItemData.Name);
                _upgradesHandler.UpgradePurchased(data.UpgradeType);
            }
        }

        private void CloseShop() => Close();

        private void OnDestroy() => 
            _tabGroup.ItemSelected -= OnItemSelected;
    }
}