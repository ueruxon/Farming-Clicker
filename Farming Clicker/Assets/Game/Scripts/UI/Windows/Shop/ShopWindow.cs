using System;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.Shop
{
    public class ShopWindow : MonoBehaviour
    {
        public event Action ShopItemSelected; 

        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space(2)]
        [SerializeField] private ShopSection _shopSection;

        private FarmController _farmController;
        
        public void Init(IStaticDataService staticDataService, UIFactory factory, FarmController farmController)
        {
            _farmController = farmController;
            
            _shopSection.Init(staticDataService, factory);
            _shopSection.ItemClicked += OnItemClicked;

            _closeButton.onClick.AddListener(CloseShop);
        }

        public void Open()
        {
            _canvasGroup.SetActive(true);
        }

        public void Close() => 
            _canvasGroup.SetActive(false);

        private void OnItemClicked(FarmData data)
        {
            if (data.DataType == ShopDataType.Product)
            {
                _farmController.CreateGhostArea(data.ProductType);
                Close();
                
                ShopItemSelected?.Invoke();
            }
        }

        private void CloseShop() => 
            Close();

        private void OnDestroy() => 
            _shopSection.ItemClicked -= OnItemClicked;
    }
}