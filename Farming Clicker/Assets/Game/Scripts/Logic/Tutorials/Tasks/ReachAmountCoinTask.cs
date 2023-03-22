using Game.Scripts.Infrastructure.Services.Progress;
using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/ReachAmountCoin", fileName = "New Task")]
    public class ReachAmountCoinTask : TutorialTask
    {
        public override void OnStart()
        {
            ResourceRepository.ResourceAmountChanged += ResourceAmountChanged;
            base.OnStart();
        }

        private void ResourceAmountChanged(ResourceType type)
        {
            int amount = ResourceRepository.StoredAmountResourceByType[type];
            
            if (amount >= 5)
                OnComplete();
        }

        public override void OnComplete()
        {
            base.OnComplete();
            ResourceRepository.ResourceAmountChanged -= ResourceAmountChanged;
        }
    }
}