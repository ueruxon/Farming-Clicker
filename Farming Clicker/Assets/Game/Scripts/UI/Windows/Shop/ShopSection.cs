using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Game.Scripts.UI.Windows.Shop
{
    public class ShopSection : MonoBehaviour
    {
        public event Action<FarmData> ItemClicked; 

        [SerializeField] private ShopNavigation _shopNavigation;
        [SerializeField] private InformationTooltip _informationTooltip;
        
        [Header("Shop Content Settings")]
        [SerializeField] private GameObject _viewportContainer;
        [SerializeField] private RectTransform _shopContentTemplate;
        [Space(2)] 
        [SerializeField] private Vector2 _openPosition;
        [SerializeField] private Vector2 _forwardHidePosition;
        [SerializeField] private Vector2 _backHidePosition;

        private IStaticDataService _staticDataService;
        private UIFactory _uiFactory;

        private List<RectTransform> _shopContents;
        private List<ShopItemData> _shopProductItems;
        private List<ShopItem> _shopItems;
        
        private int _currentContentIndex;
        
        public void Init(IStaticDataService staticDataService, UIFactory uiFactory)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;

            _currentContentIndex = 0;
            _shopContents = new List<RectTransform>();
            _shopItems = new List<ShopItem>();
            _shopProductItems = _staticDataService.GetDataForShop(ShopDataType.Product)
                .OrderBy(x => x.Price.CoinPrice)
                .ToList();

            CreateProductContent();
            CreateShopItems();
            
            _shopNavigation.Init(_shopContents.Count, _currentContentIndex);
            _shopNavigation.ContentIndexChanged += OnContentUpdate;
            
            _informationTooltip.Hide();
        }
        
        private void CreateProductContent()
        {
            for (int i = 0; i < _shopProductItems.Count; i++)
            {
                if (i % 6 == 0)
                {
                    RectTransform content = Instantiate(_shopContentTemplate, _viewportContainer.transform);
                    content.gameObject.SetActive(false);
                    content.anchoredPosition = _forwardHidePosition;

                    _shopContents.Add(content);
                }
            }
            
            _shopContentTemplate.gameObject.SetActive(false);

            if (_shopContents.Count > 0)
            {
                _shopContents[0].anchoredPosition = _openPosition;
                _shopContents[0].gameObject.SetActive(true);
            }
        }

        private void CreateShopItems()
        {
            int contentIndex = 0;
            for (int i = 0; i < _shopProductItems.Count; i++)
            {
                if (i > 0 && i % 6 == 0) 
                    contentIndex++;

                ShopItemData shopItemData = _shopProductItems[i];
                RectTransform content = _shopContents[contentIndex];
                ShopItem shopItem = _uiFactory.CreateShopItem(shopItemData, content.transform, ShopDataType.Product);
                shopItem.Clicked += OnShopItemClicked;
                shopItem.Hovered += OnShopItemHovered;

                _shopItems.Add(shopItem);
            }
        }

        private void OnShopItemClicked(FarmData data) => 
            ItemClicked?.Invoke(data);

        private void OnShopItemHovered(ShopItemData shopData, bool hover)
        {
            if (hover)
                _informationTooltip.ShowInformation(shopData.Name, shopData.Description);
            else
                _informationTooltip.Hide();
        }

        private void OnContentUpdate(NavigationMode navigationMode, int nextIndex)
        {
            RectTransform prevContent = _shopContents[_currentContentIndex];
            RectTransform content = _shopContents[nextIndex];
            
            StartCoroutine(ChangeContentAnimationRoutine(navigationMode, prevContent, content));

            _currentContentIndex = nextIndex;
        }
        
        private IEnumerator ChangeContentAnimationRoutine(NavigationMode navigationMode, 
            RectTransform prevContent, RectTransform nextContent)
        {
            float currentTime = 0;
            float interactiveDuration = .3f;
            
            nextContent.gameObject.SetActive(true);

            Vector2 startPositionForNextContent =
                navigationMode == NavigationMode.Forward ? _forwardHidePosition : _backHidePosition;
            Vector2 startPositionForPrevContent = _openPosition;

            Vector2 endPositionForNexContent = _openPosition;
            Vector2 endPositionForPrevContent = 
                navigationMode == NavigationMode.Forward ? _backHidePosition : _forwardHidePosition;
            
            while (currentTime < interactiveDuration)
            {
                float t = currentTime / interactiveDuration;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                prevContent.anchoredPosition = Vector2.Lerp(startPositionForPrevContent, endPositionForPrevContent, t);
                nextContent.anchoredPosition = Vector2.Lerp(startPositionForNextContent, endPositionForNexContent, t);
                currentTime += Time.deltaTime;
                yield return null;
            }

            prevContent.anchoredPosition = endPositionForPrevContent;
            nextContent.anchoredPosition = endPositionForNexContent;
            
            prevContent.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _shopNavigation.ContentIndexChanged -= OnContentUpdate;

            foreach (ShopItem shopItem in _shopItems)
            {
                shopItem.Clicked -= OnShopItemClicked;
                shopItem.Hovered -= OnShopItemHovered;
            }
        }
    }
}