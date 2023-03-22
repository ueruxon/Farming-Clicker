using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/WaitingCropsGrowing", fileName = "New Task")]
    public class WaitingCropsGrowingTask : TutorialTask
    {
        public override void OnStart()
        {
            FarmController.ProductionAreaStateChanged += OnProductionAreaStateChanged;
            base.OnStart();
        }

        private void OnProductionAreaStateChanged(ProductionState state)
        {
            if (state == ProductionState.Complete) 
                OnComplete();
        }

        public override void OnComplete()
        {
            base.OnComplete();
            FarmController.ProductionAreaStateChanged -= OnProductionAreaStateChanged;
        }
    }
}