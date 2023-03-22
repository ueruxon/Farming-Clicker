using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/BuyWheatCrops", fileName = "New Task")]
    public class BuyWheatCropsTask : TutorialTask
    {
        public override void OnStart()
        {
            FarmController.ProductionAreaChoices += OnProductionAreaChoices;
            base.OnStart();
        }
        
        private void OnProductionAreaChoices(ProductType productType)
        {
            if (productType == ProductType.Wheat) 
                OnComplete();
        }

        public override void OnComplete()
        {
            base.OnComplete();
            FarmController.ProductionAreaChoices -= OnProductionAreaChoices;
        }
    }
}