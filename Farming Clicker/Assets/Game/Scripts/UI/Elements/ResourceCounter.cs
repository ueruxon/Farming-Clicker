using System.Collections.Generic;
using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Services.Progress;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Elements
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _seedCounter;
        [SerializeField] private TMP_Text _coinCounter;

        private ResourceRepository _resourceRepository;
        
        public void Init(IGameProgressService progressService)
        {
            _resourceRepository = progressService.Progress.ResourceRepository;
            _resourceRepository.ResourceAmountChanged += UpdateCounters;

            foreach (KeyValuePair<ResourceType,int> pair in _resourceRepository.StoredAmountResourceByType)
                UpdateCounters(pair.Key);
        }

        private void UpdateCounters(ResourceType type)
        {
            int amount = _resourceRepository.StoredAmountResourceByType[type];
            
            switch (type)
            {
                case ResourceType.Seed:
                    _seedCounter.SetText(amount.ToString());
                    break;
                case ResourceType.Coin:
                    _coinCounter.SetText(amount.ToString());
                    break;
            }
        }

        private void OnDestroy() => 
            _resourceRepository.ResourceAmountChanged -= UpdateCounters;
    }
}