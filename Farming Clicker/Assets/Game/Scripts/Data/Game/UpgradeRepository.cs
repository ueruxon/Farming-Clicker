using System;
using System.Collections.Generic;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Logic.Upgrades;

namespace Game.Scripts.Data.Game
{
    public class UpgradeRepository
    {
        public event Action<UpgradeItemData, UpgradeItemData> UpgradeInGroupChanged;
        public event Action<UpgradeItemData> LastUpgradeInGroupPurchased;
        public event Action UpgradePurchased;
        
        private readonly Dictionary<UpgradeGroup, List<Upgrade>> _upgradesByUpgradeGroup;

        public UpgradeRepository()
        {
            _upgradesByUpgradeGroup = new Dictionary<UpgradeGroup, List<Upgrade>>
            {
                [UpgradeGroup.Gathering] = new (),
                [UpgradeGroup.Watering] = new (),
                [UpgradeGroup.FarmExpansion] = new (),
                [UpgradeGroup.Other] = new (),
            };
        }

        public void AddPurchasedUpgradeItem(Upgrade upgrade)
        {
            _upgradesByUpgradeGroup[upgrade.GetUpgradeData().UpgradeGroup].Add(upgrade);
            UpgradePurchased?.Invoke();
        }

        public void SetNextUpgradeInGroup(UpgradeItemData prevUpgrade, UpgradeItemData nextUpgrade) => 
            UpgradeInGroupChanged?.Invoke(prevUpgrade, nextUpgrade);

        public void SetLastUpgradeInGroup(UpgradeItemData lastUpgrade) => 
            LastUpgradeInGroupPurchased?.Invoke(lastUpgrade);

        public List<Upgrade> GetPurchasedUpgrades(UpgradeGroup upgradeGroup) => 
            _upgradesByUpgradeGroup[upgradeGroup];
    }
}