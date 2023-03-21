using System;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Production;
using Game.Scripts.Logic.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.HUD.Elements
{
    public enum InteractionState
    {
        None,
        Watering,
        Harvesting
    }
    
    public class InteractionCropsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private InteractionButtonSettings _settings;

        private UpgradeRepository _upgradeRepository;
        private FarmController _farmController;

        private bool _canWatering;
        private bool _canHarvesting;

        private InteractionState _interactionState;
        
        public void Init(IGameProgressService progressService, FarmController farmController)
        {
            _farmController = farmController;
            _farmController.ProductionAreaStateChanged += OnProductionAreaStateChanged;

            _upgradeRepository = progressService.Progress.UpgradeRepository;
            _upgradeRepository.UpgradePurchased += OnUpgradePurchased;
            _button.onClick.AddListener(OnInteraction);
            
            SetButtonState(InteractionState.None);
        }

        private void OnUpgradePurchased()
        {
            if (_canWatering == false)
            {
                foreach (Upgrade upgrade in _upgradeRepository.GetPurchasedUpgrades(UpgradeGroup.Watering))
                {
                    if (upgrade.GetUpgradeData().UpgradeType == UpgradeType.WaterHose)
                    {
                        _canWatering = true;
                        
                        if (HasAnyAreaInState(ProductionState.Complete))
                        {
                            if (_canHarvesting)
                                SetButtonState(InteractionState.Harvesting);
                        }
                        else if (HasAnyAreaInState(ProductionState.Idle))
                            if (_canWatering)
                                SetButtonState(InteractionState.Watering);
                    }
                }
            }

            if (_canHarvesting == false)
            {
                foreach (Upgrade upgrade in _upgradeRepository.GetPurchasedUpgrades(UpgradeGroup.Gathering))
                {
                    if (upgrade.GetUpgradeData().UpgradeType == UpgradeType.Scythe)
                    {
                        _canHarvesting = true;
                        
                        if (HasAnyAreaInState(ProductionState.Complete))
                            if (_canHarvesting)
                                SetButtonState(InteractionState.Harvesting);
                    }
                }
            }
        }
        
        private void OnProductionAreaStateChanged(ProductionState state)
        {
            if (state is ProductionState.Complete or ProductionState.Idle or ProductionState.Destroyed)
            {
                if (HasAnyAreaInState(ProductionState.Complete))
                {
                    if (_canHarvesting)
                    {
                        SetButtonState(InteractionState.Harvesting);
                        return;
                    }
                }
                
                if (HasAnyAreaInState(ProductionState.Idle))
                {
                    if (_canWatering)
                    {
                        SetButtonState(InteractionState.Watering);
                        return;
                    }
                }

                SetButtonState(InteractionState.None);
            }
            
            if (state == ProductionState.Grow)
            {
                if (HasAnyAreaInState(ProductionState.Complete) == false
                    && HasAnyAreaInState(ProductionState.Idle) == false)
                {
                    SetButtonState(InteractionState.None);
                }
            }
        }
        
        private void OnInteraction()
        {
            if (_canHarvesting && _interactionState == InteractionState.Harvesting)
            {
                _farmController.HarvestAllCrops();
                SetButtonState(HasAnyAreaInState(ProductionState.Idle) 
                    ? InteractionState.Watering 
                    : InteractionState.None);
                
                return;
            }
            
            if (_canWatering && _interactionState == InteractionState.Watering)
            {
                _farmController.WaterAllCrops();
                SetButtonState(InteractionState.None);
            }
        }

        private bool HasAnyAreaInState(ProductionState state)
        {
            foreach (ProductionArea area in _farmController.GetAllProductionsArea())
                if (area.GetProductionState() == state)
                    return true;

            return false;
        }

        private void SetButtonState(InteractionState newState)
        {
            _interactionState = newState;
            UpdateView(_interactionState);
        }

        private void UpdateView(InteractionState interactionState)
        {
            switch (interactionState)
            {
                case InteractionState.None:
                    _button.gameObject.SetActive(false);
                    break;
                case InteractionState.Watering:
                    _background.color = _settings.WaterBackgroundColor;
                    _label.SetText(_settings.WaterLabel);
                    _button.gameObject.SetActive(true);
                    break;
                case InteractionState.Harvesting:
                    _background.color = _settings.HarvestBackgroundColor;
                    _label.SetText(_settings.HarvestLabel);
                    _button.gameObject.SetActive(true);
                    break;
            }
        }

        private void OnDestroy()
        {
            _farmController.ProductionAreaStateChanged -= OnProductionAreaStateChanged;
            _upgradeRepository.UpgradePurchased -= OnUpgradePurchased;
            _button.onClick.RemoveListener(OnInteraction);
        }
    }

    [Serializable]
    public class InteractionButtonSettings
    {
        public string WaterLabel;
        public Color WaterBackgroundColor;
        public string HarvestLabel;
        public Color HarvestBackgroundColor;
    }
}