using System;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.Services.Progress;

namespace Game.Scripts.Data.Game
{
    [Serializable]
    public class GameProgress
    {
        public ResourceRepository ResourceRepository;

        public GameProgress()
        {
            ResourceRepository = new ResourceRepository();
        }
    }
    
    public class ResourceRepository
    {
        public Action<ResourceType> ResourceAmountChanged;
        public readonly Dictionary<ResourceType, int> StoredAmountResourceByType;

        public ResourceRepository()
        {
            StoredAmountResourceByType = new Dictionary<ResourceType, int>
            {
                [ResourceType.Seed] = 0,
                [ResourceType.Coin] = 0,
            };
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            StoredAmountResourceByType[resourceType] += amount;
            ResourceAmountChanged?.Invoke(resourceType);
        }

        public bool CanSpendResource(ResourceType resourceType, int amount) => 
            StoredAmountResourceByType[resourceType] >= amount;

        public void SpendResource(ResourceType resourceType, int amount)
        {
            StoredAmountResourceByType[resourceType] -= amount;
            ResourceAmountChanged?.Invoke(resourceType);
        }

        public int CheckResourceAmount(ResourceType resourceType) => 
            StoredAmountResourceByType[resourceType];
    }
}