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

        [SerializeField] private Transform _groundView;
        [SerializeField] private ProductionVisual _productionVisual;
        [SerializeField] private ProductionAreaIndicator _productionIndicator;

        private ProductItemData _productItemData;
        private ProductDropData _productDropData;

        private ProductType _productType;
        private ProductionState _currentState;
        

        public void Init(ProductItemData productItemData, float areaSize)
        {
            _productItemData = productItemData;
            _productDropData = new ProductDropData();

            _groundView.localScale = new Vector3(areaSize - 1, areaSize - 1, 0.1f);
            _productionVisual.Init(_productItemData.ViewMaterial);
            _productionIndicator.Init(this);
            
            SetState(ProductionState.Idle);
        }

        public ProductItemData GetProductData() => 
            _productItemData;

        public ProductDropData GetProductDropData() => 
            _productDropData;

        public ProductionState GetProductionState() => 
            _currentState;

        public void ActivateProduction()
        {
            SetState(ProductionState.Grow);
            StartCoroutine(GrowProductRoutine());
        }

        public void ResetProduction()
        {
            SetState(ProductionState.Idle);
            UpdateProductionVisual(0, _productItemData.GrowTime);
            // добавляем продукцию в стор
            
            _productDropData = new ProductDropData();
        }

        private IEnumerator GrowProductRoutine()
        {
            float elapsedTime = 0;

            while (elapsedTime < _productItemData.GrowTime)
            {
                elapsedTime += Time.deltaTime;
                UpdateProductionVisual(elapsedTime, _productItemData.GrowTime);
                
                yield return null;
            }
            
            UpdateProductionVisual(elapsedTime, _productItemData.GrowTime);
            GenerateProductDrop();
            SetState(ProductionState.Complete);
        }

        private void UpdateProductionVisual(float elapsedTime, float maxTime)
        {
            _productionVisual.UpdateVisual(elapsedTime, maxTime);
            GrowthProgressUpdated?.Invoke(elapsedTime, maxTime);
        }

        private void GenerateProductDrop()
        {
            int percent = Random.Range(0, 100);

            if (percent <= _productItemData.DropData.SeedDropChance)
            {
                _productDropData = new ProductDropData()
                {
                    DropType = ProductDropType.Seed,
                    DropAmount = _productItemData.DropData.SeedDropAmount
                };
            }
            else
            {
                _productDropData = new ProductDropData()
                {
                    DropType = ProductDropType.Coin,
                    DropAmount = _productItemData.DropData.CoinDropAmount
                };
            }
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