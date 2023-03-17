﻿using System;
using Game.Scripts.Data;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.Shop
{
    public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<FarmData> Clicked;
        public event Action<ShopItemData, bool> Hovered; 

        [SerializeField] private Button _button;
        [Space(2)]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _icon;
        [Space(4)]
        [SerializeField] private PriceConfig _seedConfig;
        [Space(2)]
        [SerializeField] private PriceConfig _coinConfig;

        private ShopItemData _shopItemData;
        private FarmData _farmData;
        private ResourceRepository _resourceRepository;

        public void Init(ShopItemData shopItemData, FarmData farmData, IGameProgressService progressService)
        {
            _shopItemData = shopItemData;
            _farmData = farmData;
            _resourceRepository = progressService.Progress.ResourceRepository;
            _resourceRepository.ResourceAmountChanged += ResourceAmountChanged;
            
            _button.onClick.AddListener(OnClicked);

            CreateItemView();
            UpdateView();
        }

        private void ResourceAmountChanged(ResourceType type) => 
            UpdateView();

        private void CreateItemView()
        {
            _icon.sprite = _shopItemData.Icon;
            _title.SetText(_shopItemData.Name);
            
            if (_shopItemData.Price.SeedPrice == 0) 
                _seedConfig.Container.SetActive(false);
            else
                _seedConfig.Counter.SetText(_shopItemData.Price.SeedPrice.ToString());
            
            if (_shopItemData.Price.CoinPrice == 0) 
                _coinConfig.Container.SetActive(false);
            else
                _coinConfig.Counter.SetText(_shopItemData.Price.CoinPrice.ToString());
        }

        private void UpdateView()
        {
            bool enough = ResourceEnoughCheck();

            _seedConfig.Name.color = enough ? _seedConfig.DefaultColor : _seedConfig.DangerColor;
            _seedConfig.Counter.color = enough ? _seedConfig.DefaultColor : _seedConfig.DangerColor;
            _coinConfig.Name.color = enough ? _coinConfig.DefaultColor : _coinConfig.DangerColor;
            _coinConfig.Counter.color = enough ? _coinConfig.DefaultColor : _coinConfig.DangerColor;
        }

        private bool ResourceEnoughCheck()
        {
            if (_resourceRepository.CanSpendResource(ResourceType.Seed, _shopItemData.Price.SeedPrice) == false)
                return false;
            
            if (_resourceRepository.CanSpendResource(ResourceType.Coin, _shopItemData.Price.CoinPrice) == false)
                return false;
            
            return true;
        }

        private void OnClicked()
        {
            if (ResourceEnoughCheck())
            {
                _resourceRepository.SpendResource(ResourceType.Seed, _shopItemData.Price.SeedPrice);
                _resourceRepository.SpendResource(ResourceType.Coin, _shopItemData.Price.CoinPrice);
                
                Clicked?.Invoke(_farmData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => 
            Hovered?.Invoke(_shopItemData, true);

        public void OnPointerExit(PointerEventData eventData) => 
            Hovered?.Invoke(_shopItemData, false);

        private void OnDestroy() => 
            _button.onClick.RemoveListener(OnClicked);
    }
    
    [Serializable]
    public class PriceConfig
    {
        public GameObject Container;
        public TMP_Text Name;
        public TMP_Text Counter;
        public Color DefaultColor;
        public Color DangerColor;
    }
}