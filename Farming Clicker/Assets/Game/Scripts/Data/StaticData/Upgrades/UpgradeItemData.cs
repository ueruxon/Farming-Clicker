using UnityEngine;

namespace Game.Scripts.Data.StaticData.Upgrades
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/New Upgrade")]
    public class UpgradeItemData : ShopItemData
    {
        public UpgradeType UpgradeType;
        public UpgradeGroup UpgradeGroup;
    }
}