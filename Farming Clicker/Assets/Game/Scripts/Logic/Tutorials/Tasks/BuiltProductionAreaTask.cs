using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/BuiltProductionArea", fileName = "New Task")]
    public class BuiltProductionAreaTask : TutorialTask
    {
        public override void OnStart()
        {
            FarmController.ProductionAreaBuilt += OnProductionAreaBuilt;
            base.OnStart();
        }

        private void OnProductionAreaBuilt()
        {
            
            OnComplete();
        }

        public override void OnComplete()
        {
            base.OnComplete();
            FarmController.ProductionAreaBuilt -= OnProductionAreaBuilt;
        }
    }
}