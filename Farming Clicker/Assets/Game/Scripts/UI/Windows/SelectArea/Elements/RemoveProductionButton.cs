using System;
using Game.Scripts.Data.Game;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea.Elements
{
    public class RemoveProductionButton : MonoBehaviour
    {
        public event Action ButtonClicked;
        
        [SerializeField] private Button _button;

        private UpgradeRepository _upgradeRepository;

        public void Init(IGameProgressService progressService)
        {
            _upgradeRepository = progressService.Progress.UpgradeRepository;
            _upgradeRepository.UpgradePurchased += OnUpgradePurchased;

            _button.interactable = false;
            _button.onClick.AddListener(OnRemove);
            _button.gameObject.SetActive(false);
        }

        private void OnUpgradePurchased()
        {
            foreach (Upgrade upgrade in _upgradeRepository.GetPurchasedUpgrades(UpgradeGroup.Other))
            {
                if (upgrade.GetUpgradeData().UpgradeType == UpgradeType.Shovel)
                {
                    _button.gameObject.SetActive(true);
                    _button.interactable = true;
                }
            }
        }

        private void OnRemove() => 
            ButtonClicked?.Invoke();

        private void OnDestroy()
        {
            _upgradeRepository.UpgradePurchased -= OnUpgradePurchased;
            _button.onClick.RemoveListener(OnRemove);
        }
    }
}