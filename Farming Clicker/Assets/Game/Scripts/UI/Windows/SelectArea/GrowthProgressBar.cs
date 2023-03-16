using System;
using Game.Scripts.Logic.Production;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea
{
    public class GrowthProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        private ProductionArea _productionArea;
        
        public void UpdateProgress(ProductionArea productionArea)
        {
            if (_productionArea is not null)
            {
                _fillImage.fillAmount = 0;
                _productionArea.GrowthProgressUpdated -= SetValue;
            }

            _productionArea = productionArea;
            _productionArea.GrowthProgressUpdated += SetValue;

            if (_productionArea.GetProductionState() == ProductionState.Complete)
                _fillImage.fillAmount = 1;
        }

        private void SetValue(float current, float max) => 
            _fillImage.fillAmount = current / max;

        public void Stop()
        {
            if (_productionArea is not null)
            {
                _productionArea.GrowthProgressUpdated -= SetValue;
                _fillImage.fillAmount = 0;
            }
        }
    }
}