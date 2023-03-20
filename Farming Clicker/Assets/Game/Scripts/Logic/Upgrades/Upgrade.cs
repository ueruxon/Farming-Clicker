using Game.Scripts.Data.StaticData.Upgrades;

namespace Game.Scripts.Logic.Upgrades
{
    public class Upgrade
    {
        private readonly UpgradeItemData _upgradeData;

        public Upgrade(UpgradeItemData upgradeData)
        {
            _upgradeData = upgradeData;
        }

        public UpgradeItemData GetUpgradeData() => _upgradeData;
    }
}