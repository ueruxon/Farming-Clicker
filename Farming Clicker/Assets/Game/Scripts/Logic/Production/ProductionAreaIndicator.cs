using System;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Infrastructure.Services.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Logic.Production
{
    public class ProductionAreaIndicator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _waterButton;
        [SerializeField] private Button _coinButton;
        [SerializeField] private Button _seedButton;
        [SerializeField] private TMP_Text _counter;

        private ProductionArea _productionArea;

        public void Init(ProductionArea productionArea)
        {
            _productionArea = productionArea;
            _productionArea.StateChanged += OnProductionStateChanged;
            
            _waterButton.onClick.AddListener(_productionArea.ActivateProduction);
            _coinButton.onClick.AddListener(_productionArea.ResetProduction);
            _seedButton.onClick.AddListener(_productionArea.ResetProduction);
        }

        public void Show() => 
            _canvasGroup.SetActive(true);

        public void Hide() => 
            _canvasGroup.SetActive(false);

        private void OnProductionStateChanged(ProductionState state)
        {
            HideAllButtons();

            switch (state)
            {
                case ProductionState.Idle:
                    _waterButton.gameObject.SetActive(true);
                    break;
                case ProductionState.Grow:
                    break;
                case ProductionState.Complete:
                    ProductDropData productDropData = _productionArea.GetProductDropData();
                    UpdateHarvestButton(productDropData);
                    break;
            }
        }

        private void UpdateHarvestButton(ProductDropData dropData)
        {
            _counter.SetText($"{dropData.DropAmount.ToString()}");
            _counter.gameObject.SetActive(dropData.DropAmount > 1);
            
            // сюда проверка апгрейдов?
            if (dropData.DropType == ResourceType.Seed) 
                _seedButton.gameObject.SetActive(true);
            if (dropData.DropType == ResourceType.Coin) 
                _coinButton.gameObject.SetActive(true);
        }

        private void HideAllButtons()
        {
            _counter.gameObject.SetActive(false);
            _waterButton.gameObject.SetActive(false);
            _coinButton.gameObject.SetActive(false);
            _seedButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _waterButton.onClick.RemoveListener(_productionArea.ActivateProduction);
            _coinButton.onClick.RemoveListener(_productionArea.ResetProduction);
            _seedButton.onClick.RemoveListener(_productionArea.ResetProduction);
        }
    }
}