using System;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
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
        [Space(2)]
        [SerializeField] private GameObject _seedContainer;
        [SerializeField] private TMP_Text _seedCount;
        [Space(2)]
        [SerializeField] private GameObject _coinContainer;
        [SerializeField] private TMP_Text _coinCount;

        private ShopItemData _shopItemData;
        private FarmData _farmData;

        public void Init(ShopItemData shopItemData, FarmData farmData)
        {
            _shopItemData = shopItemData;
            _farmData = farmData;
            
            _button.onClick.AddListener(OnClicked);
            UpdateView();
        }

        private void UpdateView()
        {
            _icon.sprite = _shopItemData.Icon;
            _title.SetText(_shopItemData.Name);

            if (_shopItemData.Price.SeedPrice == 0) 
                _seedContainer.SetActive(false);
            else
                _seedCount.SetText(_shopItemData.Price.SeedPrice.ToString());

            if (_shopItemData.Price.CoinPrice == 0) 
                _coinContainer.SetActive(false);
            else
                _coinCount.SetText(_shopItemData.Price.CoinPrice.ToString());
        }

        private void OnClicked() => 
            Clicked?.Invoke(_farmData);

        public void OnPointerEnter(PointerEventData eventData) => 
            Hovered?.Invoke(_shopItemData, true);

        public void OnPointerExit(PointerEventData eventData) => 
            Hovered?.Invoke(_shopItemData, false);

        private void OnDestroy() => 
            _button.onClick.RemoveListener(OnClicked);
    }
}