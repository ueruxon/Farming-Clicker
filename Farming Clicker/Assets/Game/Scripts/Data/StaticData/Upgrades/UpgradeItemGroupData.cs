using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Data.StaticData.Upgrades
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/New Upgrade Group")]
    public class UpgradeItemGroupData : ScriptableObject
    {
        public List<UpgradeItemData> UpgradeItemsData;
        public UpgradeGroup UpgradeGroup;
    }
}