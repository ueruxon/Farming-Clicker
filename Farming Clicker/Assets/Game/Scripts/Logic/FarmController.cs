using System;
using System.Collections.Generic;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Data.StaticData.Upgrades;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Production;
using Game.Scripts.Logic.Upgrades;
using UnityEngine;

namespace Game.Scripts.Logic
{
    public enum FarmState
    {
        Select,
        Build
    }
    
    public class FarmController
    {
        public event Action<ProductType> ProductionAreaChoices;
        public event Action ProductionAreaCanceled;
        public event Action ProductionAreaBuilt;
        public event Action<ProductionState> ProductionAreaStateChanged;
        public event Action<GridCell> GridCellSelected;
        public event Action GridCellDeselected;

        private readonly GameFactory _gameFactory;
        private readonly IGameProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly GridSystem _gridSystem;
        private readonly Camera _mainCamera;

        private ProductionAreaGhost _areaGhost;
        private ProductType _activeProductType;

        private FarmState _farmState;
        private GridCell _selectedCell;

        private List<ProductionArea> _productionAreas;

        public FarmController(IGameProgressService progressService, 
            IStaticDataService staticDataService, 
            GameFactory gameFactory, 
            GridSystem gridSystem)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _gridSystem = gridSystem;
            _mainCamera = Camera.main;

            _productionAreas = new List<ProductionArea>();
        }

        public void Init() => 
            _progressService.Progress.UpgradeRepository.UpgradePurchased += OnSomeUpgradePurchased;

        public void CreateFarm()
        {
            GridCell[,] grid = _gridSystem.CreateGrid();
            foreach (GridCell cell in grid)
            {
                cell.Enter += OnCellEnter;
                cell.Exit += OnCellExit;
                cell.Clicked += OnCellClicked;
            }

            SetState(FarmState.Select);
        }

        public List<ProductionArea> GetAllProductionsArea() => 
            _productionAreas;

        public void BuildProductionArea(ProductType productType)
        {
            SetState(FarmState.Build);
            _activeProductType = productType;

            CreateGhostArea();
        }

        public void CancelConstruction()
        {
            ReturnResourceForConstruction(_activeProductType);
            ClearAreaGhost();
            
            ProductionAreaCanceled?.Invoke();
        }

        public void DeselectArea()
        {
            SetState(FarmState.Select);
            _selectedCell.Select(false);
            _selectedCell = null;
            
            GridCellDeselected?.Invoke();
        }

        public void RemoveSelectedProductionArea()
        {
            if (_selectedCell != null)
            {
                ProductionArea area = _selectedCell.GetProductionArea();
                _productionAreas.Remove(area);
                _selectedCell.ClearProductionArea();
                
                area.StateChanged -= OnProductionAreaStateChanged;
            }
        }

        public void WaterAllCrops()
        {
            foreach (ProductionArea area in _productionAreas)
            {
                if (area.GetProductionState() == ProductionState.Idle)
                    area.ActivateProduction();
            }
        }

        public void HarvestAllCrops()
        {
            foreach (ProductionArea area in _productionAreas)
            {
                if (area.GetProductionState() == ProductionState.Complete)
                    area.HarvestProduction();
            }
        }
        
        private void OnSomeUpgradePurchased()
        {
            List<Upgrade> upgrades = 
                _progressService.Progress.UpgradeRepository.GetPurchasedUpgrades(UpgradeGroup.FarmExpansion);

            _gridSystem.OpenGridCells(upgrades.Count);
        }

        private void ReturnResourceForConstruction(ProductType productType)
        {
            PriceAmount productPriceData = _staticDataService.GetDataForProduct(productType).Price;
            _progressService.Progress.ResourceRepository.AddResource(ResourceType.Seed, productPriceData.SeedPrice);
            _progressService.Progress.ResourceRepository.AddResource(ResourceType.Coin, productPriceData.CoinPrice);
        }

        private void CreateGhostArea()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = _mainCamera.nearClipPlane;
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(mousePosition);
            
            _areaGhost = _gameFactory.CreateAreaGhost(worldPoint);
            _gridSystem.SelectAllAvailableCell();
            
            ProductionAreaChoices?.Invoke(_activeProductType);
        }

        private void ClearAreaGhost()
        {
            _areaGhost.Destroy();
            _areaGhost = null;
            _gridSystem.DeselectAllAvailableCell();
            _activeProductType = ProductType.None;
            SetState(FarmState.Select);
        }

        private void CreateProductionArea(ProductType productType, Vector2Int cellPosition)
        {
            GridCell gridCell = _gridSystem.GetGridCell(cellPosition);
            Vector3 spawnPosition = _gridSystem.GetWorldPosition(cellPosition);
            ProductionArea productionArea = _gameFactory.CreateProductionArea(productType, at: spawnPosition);
            productionArea.StateChanged += OnProductionAreaStateChanged;
            
            gridCell.AddProductionArea(productionArea);
            _productionAreas.Add(productionArea);

            OnProductionAreaStateChanged(productionArea.GetProductionState());
        }

        private void OnCellEnter(Vector2Int cellPosition)
        {
            GridCell cell = _gridSystem.GetGridCell(cellPosition);

            if (_areaGhost is not null) 
                _areaGhost.Hook(cell.transform);
        }

        private void OnCellExit(Vector2Int cellPosition)
        {
            GridCell cell = _gridSystem.GetGridCell(cellPosition);
            
            if (_areaGhost is not null) 
                _areaGhost.Unhook(cell.transform);
        }

        private void OnCellClicked(Vector2Int cellPosition)
        {
            if (_farmState == FarmState.Build)
            {
                CreateProductionArea(_activeProductType, cellPosition);
                ClearAreaGhost();

                ProductionAreaBuilt?.Invoke();
                
                return;
            }

            if (_farmState == FarmState.Select)
            {
                if (_selectedCell is not null)
                {
                    _selectedCell.Select(false);
                    _selectedCell = null;
                }
                
                _selectedCell = _gridSystem.GetGridCell(cellPosition);
                _selectedCell.Select(true);

                GridCellSelected?.Invoke(_selectedCell);
            }
        }

        private void OnProductionAreaStateChanged(ProductionState state) => 
            ProductionAreaStateChanged?.Invoke(state);

        private void SetState(FarmState newState) => 
            _farmState = newState;
        
        public void Cleanup()
        {
            _progressService.Progress.UpgradeRepository.UpgradePurchased -= OnSomeUpgradePurchased;

            if (_productionAreas.Count > 0)
                foreach (ProductionArea productionArea in _productionAreas)
                    productionArea.StateChanged -= OnProductionAreaStateChanged;
            
            _productionAreas.Clear();
        }
    }
}