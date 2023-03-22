using Game.Scripts.Logic.GridLayout;
using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/SelectProductionArea", fileName = "New Task")]
    public class SelectProductionAreaTask : TutorialTask
    {
        public override void OnStart()
        {
            FarmController.GridCellSelected += OnGridCellSelected;
            base.OnStart();
        }

        private void OnGridCellSelected(GridCell cell) => 
            OnComplete();

        public override void OnComplete()
        {
            base.OnComplete();
            FarmController.GridCellSelected -= OnGridCellSelected;
        }
    }
}