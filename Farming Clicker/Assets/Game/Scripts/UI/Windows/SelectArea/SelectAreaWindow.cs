using Game.Scripts.Common.Extensions;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Production;
using Game.Scripts.UI.Windows.SelectArea.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea
{
    public class SelectAreaWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RemoveProductionButton _removeProductionButton;
        [SerializeField] private Button _backButton;

        [Header("Product Settings")]
        [Space(2)]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private ProductionCycleContent _productionCycle;
        [SerializeField] private GrowthProgressBar _growthProgressBar;

        private FarmController _farmController;
        
        
        public void Init(IGameProgressService progressService, FarmController farmController)
        {
            _farmController = farmController;
            _farmController.GridCellSelected += OnProductionAreaSelected;
            
            _productionCycle.Init();
            _removeProductionButton.Init(progressService);
            _removeProductionButton.ButtonClicked += OnProductionAreaRemoved;
            
            _backButton.onClick.AddListener(OnProductionAreaDeselect);
        }

        public void Hide() => 
            _canvasGroup.SetActive(false);

        private void OnProductionAreaSelected(GridCell cell)
        {
            ProductionArea productionArea = cell.GetProductionArea();
            
            _title.SetText(productionArea.GetProductData().Name);
            _productionCycle.SetProductionArea(productionArea);
            _growthProgressBar.UpdateProgress(productionArea);

            Show();
        }

        private void OnProductionAreaRemoved()
        {
            _farmController.RemoveSelectedProductionArea();
            OnProductionAreaDeselect();
        }

        private void OnProductionAreaDeselect()
        {
            _farmController.DeselectArea();
            _growthProgressBar.Stop();
            
            Hide();
        }

        private void Show() => 
            _canvasGroup.SetActive(true);

        private void OnDestroy()
        {
            _farmController.GridCellSelected -= OnProductionAreaSelected;
            _removeProductionButton.ButtonClicked -= OnProductionAreaRemoved;
            _backButton.onClick.RemoveListener(OnProductionAreaDeselect);
        }
    }
}