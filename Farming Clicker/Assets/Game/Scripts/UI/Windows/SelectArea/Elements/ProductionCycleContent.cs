using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.Production;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea.Elements
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
            // отписываемся от предыдущей area?
            if (_productionArea is not null)
            {
                if (Equals(_productionArea, productionArea))
                    return;

                _productionArea.StateChanged -= OnProductionStateChanged;
            }

            _productionArea = productionArea;
            _productionArea.StateChanged += OnProductionStateChanged;

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
                    ProductDropData productDropData = _productionArea.GetProductDropData();
                    UpdateHarvestButton(productDropData);
                    break;
            }
        }
        
        private void UpdateHarvestButton(ProductDropData dropData)
        {
            if (dropData.DropType == ResourceType.Seed)
            {
                _seedCounter.SetText($"{dropData.DropAmount.ToString()} SEEDS READY");
                _harvestSeedButton.gameObject.SetActive(true);
            }
            
            if (dropData.DropType == ResourceType.Coin)
            {
                _coinCounter.SetText($"{dropData.DropAmount.ToString()} COINS READY");
                _harvestCoinButton.gameObject.SetActive(true);
            }
        }
        
        private void OnWaterClick() => 
            _productionArea.ActivateProduction();

        private void OnCoinClick() => 
            _productionArea.HarvestProduction();

        private void OnSeedClick() => 
            _productionArea.HarvestProduction();

        private void HideAllButtons()
        {
            _growingText.gameObject.SetActive(false);
            _waterButton.gameObject.SetActive(false);
            _harvestCoinButton.gameObject.SetActive(false);
            _harvestSeedButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_productionArea is not null)
                _productionArea.StateChanged -= OnProductionStateChanged;
            
            _waterButton.onClick.RemoveListener(OnWaterClick);
            _harvestCoinButton.onClick.RemoveListener(OnCoinClick);
            _harvestSeedButton.onClick.RemoveListener(OnSeedClick);
        }
    }
}