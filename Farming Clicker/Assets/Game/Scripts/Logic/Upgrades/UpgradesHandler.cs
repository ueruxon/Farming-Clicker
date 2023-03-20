using System.Collections.Generic;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;

namespace Game.Scripts.Logic.Upgrades
{
    public class UpgradesHandler
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameProgressService _progressService;

        private Dictionary<UpgradeGroup, Queue<UpgradeItemData>> _allAvailableUpgradesQueue;
        private List<UpgradeItemData> _currentAvailableUpgrades;

        public UpgradesHandler(IStaticDataService staticDataService, IGameProgressService progressService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            
            _allAvailableUpgradesQueue = new Dictionary<UpgradeGroup, Queue<UpgradeItemData>>();
            _currentAvailableUpgrades = new List<UpgradeItemData>();
        }

        public void Init()
        {
            Dictionary<UpgradeGroup, List<UpgradeItemData>> upgradesDataGroup = _staticDataService.GetUpgradesByGroup();

            foreach (var pair in upgradesDataGroup)
            {
                Queue<UpgradeItemData> queue = new Queue<UpgradeItemData>();
                foreach (UpgradeItemData upgradeItemData in pair.Value) 
                    queue.Enqueue(upgradeItemData);
                
                _allAvailableUpgradesQueue.Add(pair.Key, queue);
            }

            foreach (var pair in _allAvailableUpgradesQueue)
            {
                if (pair.Value.TryDequeue(out UpgradeItemData data)) 
                    _currentAvailableUpgrades.Add(data);
            }
        }

        public List<UpgradeItemData> GetAvailableUpgrades() => 
            _currentAvailableUpgrades;

        public void UpgradePurchased(UpgradeType type)
        {
            UpgradeItemData upgradeData = _staticDataService.GetDataForUpgrade(type);
            Upgrade upgrade = new Upgrade(upgradeData);
            
            _progressService.Progress.UpgradeRepository.AddPurchasedUpgradeItem(upgrade);
            
            if (HasNextUpgrade(upgradeData.UpgradeGroup))
            {
                UpgradeItemData nextUpgrade = GetNextUpgrade(upgradeData.UpgradeGroup);
                _progressService.Progress.UpgradeRepository.SetNextUpgradeInGroup(prevUpgrade: upgradeData, nextUpgrade);
            }
            else
                _progressService.Progress.UpgradeRepository.SetLastUpgradeInGroup(upgradeData);
        }

        private bool HasNextUpgrade(UpgradeGroup upgradeGroup) => 
            _allAvailableUpgradesQueue[upgradeGroup].Count > 0;

        private UpgradeItemData GetNextUpgrade(UpgradeGroup upgradeGroup) => 
            _allAvailableUpgradesQueue[upgradeGroup].Dequeue();
    }
}