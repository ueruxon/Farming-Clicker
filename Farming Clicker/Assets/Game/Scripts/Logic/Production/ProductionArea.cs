using System.Collections;
using Game.Scripts.Data.StaticData.Product;
using UnityEngine;

namespace Game.Scripts.Logic.Production
{

    public enum ProductionState
    {
        Idle,
        Grow,
        Complete
    }
    
    public class ProductionArea : MonoBehaviour
    {
        [SerializeField] private Transform _groundView;
        [SerializeField] private ProductionVisual _productionVisual;

        private ProductItemData _productItemData;
        
        private ProductType _productType;
        private ProductionState _currentState;

        public void Init(ProductItemData productItemData, float areaSize)
        {
            _productItemData = productItemData;
            _currentState = ProductionState.Idle;
            _groundView.localScale = new Vector3(areaSize - 1, areaSize - 1, 0.1f);
            
            _productionVisual.Init(_productItemData.ViewMaterial);
        }

        public void ActivateProduction()
        {
            _currentState = ProductionState.Grow;
            
            StartCoroutine(GrowProductRoutine());
        }

        private IEnumerator GrowProductRoutine()
        {
            float elapsedTime = 0;

            while (elapsedTime < _productItemData.GrowTime)
            {
                elapsedTime += Time.deltaTime;
                _productionVisual.UpdateVisual(elapsedTime, _productItemData.GrowTime);
                yield return null;
            }
            
            _productionVisual.UpdateVisual(elapsedTime, _productItemData.GrowTime);
            _currentState = ProductionState.Complete;
        }
    }
}