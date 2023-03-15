using System;
using System.Collections;
using Game.Scripts.Data.StaticData.Product;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public event Action<float, float> GrowthProgressUpdated;
        public event Action<ProductionState> StateChanged;
        public event Action<ProductDropData> GrowthCompleted;
        
        [SerializeField] private Transform _groundView;
        [SerializeField] private ProductionVisual _productionVisual;

        private ProductItemData _productItemData;

        private ProductType _productType;
        private ProductionState _currentState;

        public void Init(ProductItemData productItemData, float areaSize)
        {
            _productItemData = productItemData;
            _groundView.localScale = new Vector3(areaSize - 1, areaSize - 1, 0.1f);
            _productionVisual.Init(_productItemData.ViewMaterial);
            
            SetState(ProductionState.Idle);
        }

        public ProductItemData GetProductData() => 
            _productItemData;

        public ProductionState GetProductionState() => 
            _currentState;

        public void ActivateProduction()
        {
            SetState(ProductionState.Grow);
            StartCoroutine(GrowProductRoutine());
        }

        private IEnumerator GrowProductRoutine()
        {
            float elapsedTime = 0;

            while (elapsedTime < _productItemData.GrowTime)
            {
                elapsedTime += Time.deltaTime;
                _productionVisual.UpdateVisual(elapsedTime, _productItemData.GrowTime);
                GrowthProgressUpdated?.Invoke(elapsedTime,  _productItemData.GrowTime);
                yield return null;
            }
            
            _productionVisual.UpdateVisual(elapsedTime, _productItemData.GrowTime);
            GrowthProgressUpdated?.Invoke(elapsedTime, _productItemData.GrowTime);

            GenerateProductDrop();
        }

        private void GenerateProductDrop()
        {
            int percent = Random.Range(0, 100);

            if (percent <= _productItemData.DropData.SeedDropChance)
            {
                ProductDropData productDropData = new ProductDropData()
                {
                    DropType = ProductDropType.Seed,
                    DropAmount = _productItemData.DropData.SeedDropAmount
                };

                GrowthCompleted?.Invoke(productDropData);
            }
            else
            {
                ProductDropData productDropData = new ProductDropData()
                {
                    DropType = ProductDropType.Coin,
                    DropAmount = _productItemData.DropData.CoinDropAmount
                };

                GrowthCompleted?.Invoke(productDropData);
            }
            
            SetState(ProductionState.Complete);
        }

        private void SetState(ProductionState newState)
        {
            _currentState = newState;
            StateChanged?.Invoke(_currentState);
        }
    }

    public class ProductDropData
    {
        public ProductDropType DropType;
        public int DropAmount;
    }
}