using System;
using System.Collections.Generic;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Game.Scripts.UI.Windows.Shop.Elements
{
    public class TabGroup : MonoBehaviour
    {
        public event Action<TabContentType, ShopItemData> ItemSelected;
        
        [SerializeField] private List<TabButton> _tabButtons;
        [SerializeField] private List<TabSection> _tabSections;

        private TabContentType _activeTab;

        public void Init(IStaticDataService staticDataService, UIFactory factory, UpgradesHandler upgradesHandler)
        {
            foreach (TabButton tabButton in _tabButtons)
            {
                tabButton.Init();
                tabButton.TabButtonClicked += OnTabSelected;
            }

            foreach (TabSection tabSection in _tabSections)
            {
                tabSection.Init(staticDataService, factory, upgradesHandler);
                tabSection.Hide();
                tabSection.ItemClicked += OnItemSelected;
            }

            OnTabSelected(TabContentType.Products);
        }

        private void OnTabSelected(TabContentType tabType)
        {
            if (_activeTab == tabType)
                return;

            foreach (TabSection tabSection in _tabSections)
            {
                tabSection.Hide();
                
                if (tabSection.GetTabContentType() == tabType)
                {
                    tabSection.Show();
                    _activeTab = tabType;
                }
            }

            foreach (TabButton button in _tabButtons) 
                button.Selection(tabType);
        }

        private void OnItemSelected(TabContentType contentType, ShopItemData shopItemData) => 
            ItemSelected?.Invoke(contentType, shopItemData);

        private void OnDestroy()
        {
            foreach (TabButton tabButton in _tabButtons) 
                tabButton.TabButtonClicked -= OnTabSelected;
            
            foreach (TabSection tabSection in _tabSections) 
                tabSection.ItemClicked -= OnItemSelected;
        }
    }
}