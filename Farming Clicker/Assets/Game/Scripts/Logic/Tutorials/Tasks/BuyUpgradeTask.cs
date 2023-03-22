using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Logic.Upgrades;
using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/BuyUpgrade", fileName = "New Task")]
    public class BuyUpgradeTask : TutorialTask
    {
        public override void OnStart()
        {
            UpgradeRepository.UpgradePurchased += OnUpgradePurchased;
            base.OnStart();
        }

        private void OnUpgradePurchased()
        {
            foreach (Upgrade upgrade in UpgradeRepository.GetPurchasedUpgrades(UpgradeGroup.Gathering))
            {
                if (upgrade.GetUpgradeData().UpgradeType == UpgradeType.Sickle) 
                    OnComplete();
            }
        }

        public override void OnComplete()
        {
            base.OnComplete();
            UpgradeRepository.UpgradePurchased -= OnUpgradePurchased;
        }
    }
}