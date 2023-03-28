using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Game.Scripts.UI.Windows.Shop.Elements
{
    public class TabSection : MonoBehaviour
    {
        public event Action<TabContentType, ShopItemData> ItemClicked;

        [SerializeField] private TabContentType _tabContentType;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space(2)]
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
        private UpgradesHandler _upgradesHandler;

        private List<RectTransform> _shopContents;
        private List<ShopItemData> _contentShopItems;
        private List<ShopItem> _allShopItems;

        private ShopDataType _dataType;
        
        private int _currentContentIndex;
        
        public void Init(IStaticDataService staticDataService, UIFactory uiFactory, UpgradesHandler upgradesHandler)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _upgradesHandler = upgradesHandler;
            _dataType = _tabContentType == TabContentType.Products ? ShopDataType.Product : ShopDataType.Upgrade;

            _currentContentIndex = 0;
            _shopContents = new List<RectTransform>();
            _allShopItems = new List<ShopItem>();
            
            CreateContentShopItems();
            CreateContent();
            CreateShopItems();
            
            _shopNavigation.Init(_shopContents.Count, _currentContentIndex);
            _shopNavigation.ContentIndexChanged += OnContentUpdate;
            _informationTooltip.Hide();
        }

        public TabContentType GetTabContentType() => _tabContentType;
        public void Show() => _canvasGroup.SetActive(true);
        public void Hide() => _canvasGroup.SetActive(false);

        private void CreateContentShopItems()
        {
            if (_dataType == ShopDataType.Product)
            {
                _contentShopItems = _staticDataService.GetDataForShop(ShopDataType.Product)
                    .OrderBy(x => x.Price.CoinPrice)
                    .ToList();
            }

            if (_dataType == ShopDataType.Upgrade)
            {
                _contentShopItems = 
                    _upgradesHandler.GetAvailableUpgrades()
                        .OrderBy(x => x.Price.CoinPrice)
                        .Select(x => x as ShopItemData)
                        .ToList();
            }
        }
        
        private void CreateContent()
        {
            for (int i = 0; i < _contentShopItems.Count; i++)
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
            for (int i = 0; i < _contentShopItems.Count; i++)
            {
                if (i > 0 && i % 6 == 0) 
                    contentIndex++;

                ShopItemData shopItemData = _contentShopItems[i];
                RectTransform content = _shopContents[contentIndex];
                ShopItem shopItem = _uiFactory.CreateShopItem(shopItemData, content.transform);
                shopItem.Clicked += OnShopItemClicked;
                shopItem.Hovered += OnShopItemHovered;

                _allShopItems.Add(shopItem);
            }
        }

        private void OnShopItemClicked(ShopItemData shopItemData) => 
            ItemClicked?.Invoke(_tabContentType, shopItemData);

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

            Vector2 endPositionForNextContent = _openPosition;
            Vector2 endPositionForPrevContent = 
                navigationMode == NavigationMode.Forward ? _backHidePosition : _forwardHidePosition;
            
            while (currentTime < interactiveDuration)
            {
                float t = currentTime / interactiveDuration;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                prevContent.anchoredPosition = Vector2.Lerp(startPositionForPrevContent, endPositionForPrevContent, t);
                nextContent.anchoredPosition = Vector2.Lerp(startPositionForNextContent, endPositionForNextContent, t);
                currentTime += Time.deltaTime;
                yield return null;
            }

            prevContent.anchoredPosition = endPositionForPrevContent;
            nextContent.anchoredPosition = endPositionForNextContent;
            
            prevContent.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _shopNavigation.ContentIndexChanged -= OnContentUpdate;

            foreach (ShopItem shopItem in _allShopItems)
            {
                shopItem.Clicked -= OnShopItemClicked;
                shopItem.Hovered -= OnShopItemHovered;
            }
        }
    }
}