using System;
using Game.Scripts.Common.Extensions;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.Upgrades;
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
        private UpgradeRepository _upgradeRepository;

        public void Init(ProductionArea productionArea, IGameProgressService gameProgressService)
        {
            _productionArea = productionArea;
            _productionArea.StateChanged += OnProductionStateChanged;

            _upgradeRepository = gameProgressService.Progress.UpgradeRepository;
            _upgradeRepository.UpgradePurchased += OnUpgradePurchased;
            
            _waterButton.onClick.AddListener(_productionArea.ActivateProduction);
            _waterButton.interactable = false;
            _coinButton.onClick.AddListener(_productionArea.ResetProduction);
            _coinButton.interactable = false;
            _seedButton.onClick.AddListener(_productionArea.ResetProduction);
            _seedButton.interactable = false;

            OnUpgradePurchased();
        }

        private void OnUpgradePurchased()
        {
            foreach (Upgrade upgrade in _upgradeRepository.GetPurchasedUpgrades(UpgradeGroup.Watering))
            {
                if (upgrade.GetUpgradeData().UpgradeType == UpgradeType.WateringCan)
                    _waterButton.interactable = true;
            }
            
            foreach (Upgrade upgrade in _upgradeRepository.GetPurchasedUpgrades(UpgradeGroup.Gathering))
            {
                if (upgrade.GetUpgradeData().UpgradeType == UpgradeType.Sickle)
                {
                    _coinButton.interactable = true;
                    _seedButton.interactable = true;
                }
            }
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