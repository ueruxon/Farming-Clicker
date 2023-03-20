using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.Shop.Elements
{
    public class TabButton : MonoBehaviour
    {
        public event Action<TabContentType> TabButtonClicked;
        
        [SerializeField] private TabContentType _contentType;
        [SerializeField] private Button _button;
        [SerializeField] private Image _tabImage;
        [SerializeField] private Color _disableTabColor;
        [SerializeField] private Color _activeTabColor;

        public void Init() => 
            _button.onClick.AddListener(OnTabClick);
        
        public void Selection(TabContentType tabType) => 
            _tabImage.color = tabType == _contentType ? _activeTabColor : _disableTabColor;

        private void OnTabClick()
        {
            TabButtonClicked?.Invoke(_contentType);
        }

        private void OnDestroy() => 
            _button.onClick.RemoveListener(OnTabClick);
    }

    public enum TabContentType
    {
        None,
        Products,
        Upgrades,
        Settings
    }
}