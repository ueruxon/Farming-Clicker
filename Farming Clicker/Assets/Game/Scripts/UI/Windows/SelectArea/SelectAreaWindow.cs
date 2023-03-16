using Game.Scripts.Common.Extensions;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Production;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea
{
    public class SelectAreaWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _backButton;

        [Header("Product Settings")]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private ProductionCycleContent _productionCycle;
        [SerializeField] private GrowthProgressBar _growthProgressBar;

        private FarmController _farmController;
        
        public void Init(FarmController farmController)
        {
            _farmController = farmController;
            _farmController.ProductAreaSelected += OnProductionAreaSelected;
            _productionCycle.Init();
            
            _backButton.onClick.AddListener(OnProductionAreaDeselect);
        }

        public void Hide() => 
            _canvasGroup.SetActive(false);

        private void OnProductionAreaSelected(ProductionArea productionArea)
        {
            _title.SetText(productionArea.GetProductData().Name);
            _productionCycle.SetProductionArea(productionArea);
            _growthProgressBar.UpdateProgress(productionArea);

            Show();
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
            _farmController.ProductAreaSelected -= OnProductionAreaSelected;
            _backButton.onClick.RemoveListener(OnProductionAreaDeselect);
        }
    }
}