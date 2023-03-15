using System;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Production;
using Game.Scripts.UI.Windows.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Elements
{
    public class OpenShopButton : MonoBehaviour
    {
        [SerializeField] private Button _openButton;

        private ShopWindow _shopWindow;
        private FarmController _farmController;
        
        public void Init(ShopWindow shopWindow, FarmController farmController)
        {
            _shopWindow = shopWindow;
            _shopWindow.ShopItemSelected += Hide;

            _farmController = farmController;
            _farmController.ProductionBuilt += Show;
            _farmController.ProductAreaSelected += OnProductAreaSelected;
            _farmController.ProductAreaDeselected += Show;
            
            _openButton.onClick.AddListener(OpenShop);
        }

        private void OpenShop() => 
            _shopWindow.Open();

        private void Hide() => 
            gameObject.SetActive(false);

        private void Show() => 
            gameObject.SetActive(true);

        private void OnProductAreaSelected(ProductionArea productionArea) => 
            Hide();

        private void OnDestroy()
        {
            _shopWindow.ShopItemSelected -= Hide;
            _farmController.ProductionBuilt -= Show;
            _farmController.ProductAreaDeselected -= Show;
            _farmController.ProductAreaSelected -= OnProductAreaSelected;
            _openButton.onClick.RemoveListener(OpenShop);
        }
    }
}