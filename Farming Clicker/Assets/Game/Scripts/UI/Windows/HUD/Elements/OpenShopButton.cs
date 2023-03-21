using System;
using Game.Scripts.Logic;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.UI.Windows.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.HUD.Elements
{
    public class OpenShopButton : MonoBehaviour
    {
        [SerializeField] private Button _openButton;

        private ShopWindow _shopWindow;
        private FarmController _farmController;
        
        public void Init(ShopWindow shopWindow, FarmController farmController)
        {
            _shopWindow = shopWindow;

            _farmController = farmController;
            _farmController.ProductionAreaChoices += Hide;
            _farmController.ProductionAreaCanceled += Show;
            _farmController.ProductionAreaBuilt += Show;
            _farmController.GridCellSelected += OnGridCellSelected;
            _farmController.GridCellDeselected += Show;
            
            _openButton.onClick.AddListener(OpenShop);
        }

        private void OpenShop() => 
            _shopWindow.Open();

        private void Hide() => 
            gameObject.SetActive(false);

        private void Show() => 
            gameObject.SetActive(true);

        private void OnGridCellSelected(GridCell cell) => 
            Hide();

        private void OnDestroy()
        {
            _farmController.ProductionAreaChoices -= Hide;
            _farmController.ProductionAreaCanceled -= Show;
            _farmController.ProductionAreaBuilt -= Show;
            _farmController.GridCellDeselected -= Show;
            _farmController.GridCellSelected -= OnGridCellSelected;
            _openButton.onClick.RemoveListener(OpenShop);
        }
    }
}