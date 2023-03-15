using System;
using Game.Scripts.Logic.Production;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea
{
    public class ProductionCycleContent : MonoBehaviour
    {
        [SerializeField] private TMP_Text _growingText;
        
        [Space(4)]
        [Header("Interactive Buttons Settings")]
        [SerializeField] private Button _waterButton;
        [Space(2)]
        [SerializeField] private Button _harvestCoinButton;
        [SerializeField] private TMP_Text _coinCounter;
        [Space(2)]
        [SerializeField] private Button _harvestSeedButton;
        [SerializeField] private TMP_Text _seedCounter;

        private ProductionArea _productionArea;

        public void Init()
        {
            _waterButton.onClick.AddListener(OnWaterClick);
            _harvestCoinButton.onClick.AddListener(OnCoinClick);
            _harvestSeedButton.onClick.AddListener(OnSeedClick);
        }

        public void SetProductionArea(ProductionArea productionArea)
        {
            if (_productionArea is not null)
            {
                _productionArea.StateChanged -= OnProductionStateChanged;
                _productionArea.GrowthCompleted -= UpdateHarvestButton;
            }
            
            _productionArea = productionArea;
            _productionArea.StateChanged += OnProductionStateChanged;
            _productionArea.GrowthCompleted += UpdateHarvestButton;
            
            OnProductionStateChanged(_productionArea.GetProductionState());
        }

        private void OnProductionStateChanged(ProductionState state)
        {
            HideAllButtons();
            
            switch (state)
            {
                case ProductionState.Idle:
                    _waterButton.gameObject.SetActive(true);
                    break;
                case ProductionState.Grow:
                    _growingText.gameObject.SetActive(true);
                    break;
                case ProductionState.Complete:
                    break;
            }
        }
        
        private void UpdateHarvestButton(ProductDropData dropData)
        {
            if (dropData.DropType == ProductDropType.Seed)
            {
                _seedCounter.SetText($"{dropData.DropAmount.ToString()} SEEDS READY");
                _harvestSeedButton.gameObject.SetActive(true);
            }
            
            if (dropData.DropType == ProductDropType.Coin)
            {
                _seedCounter.SetText($"{dropData.DropAmount.ToString()} COINS READY");
                _harvestCoinButton.gameObject.SetActive(true);
            }
        }
        
        private void OnWaterClick()
        {
            _productionArea.ActivateProduction();
        }

        private void OnCoinClick()
        {
            
        }
        
        private void OnSeedClick()
        {
            
        }

        private void HideAllButtons()
        {
            _growingText.gameObject.SetActive(false);
            _waterButton.gameObject.SetActive(false);
            _harvestCoinButton.gameObject.SetActive(false);
            _harvestSeedButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _waterButton.onClick.RemoveListener(OnWaterClick);
            _harvestCoinButton.onClick.RemoveListener(OnCoinClick);
            _harvestSeedButton.onClick.RemoveListener(OnSeedClick);
        }
    }
}