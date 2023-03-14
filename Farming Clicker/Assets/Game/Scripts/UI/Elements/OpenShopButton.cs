using System;
using Game.Scripts.Logic;
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
            
            _openButton.onClick.AddListener(OpenShop);
        }

        private void OpenShop() => 
            _shopWindow.Open();

        private void Hide() => 
            gameObject.SetActive(false);

        private void Show() => 
            gameObject.SetActive(true);

        private void OnDestroy()
        {
            _shopWindow.ShopItemSelected -= Hide;
            _farmController.ProductionBuilt -= Show;
            _openButton.onClick.RemoveListener(OpenShop);
        }
    }
}