using System;
using Game.Scripts.Data;
using Game.Scripts.Data.StaticData;
using Game.Scripts.Data.StaticData.Product;
using Game.Scripts.Infrastructure.Services.Factory;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Infrastructure.Services.StaticData;
using Game.Scripts.Logic.Cameras;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Production;
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
        public event Action ProductionAreaChoices;
        public event Action ProductionAreaCanceled;
        public event Action ProductionAreaBuilt;
        public event Action<ProductionArea> ProductAreaSelected;
        public event Action ProductAreaDeselected;

        private readonly GameFactory _gameFactory;
        private readonly IGameProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly GridSystem _gridSystem;
        private readonly Camera _mainCamera;
        
        private ProductionAreaGhost _areaGhost;
        private ProductType _activeProductType;

        private FarmState _farmState;
        private GridCell _selectedCell;

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
        }

        public void InitFarm()
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
            
            ProductAreaDeselected?.Invoke();
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
            
            ProductionAreaChoices?.Invoke();
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
            gridCell.AddProductionArea(productionArea);
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
                ProductionArea productionArea = _selectedCell.GetProductionArea();

                ProductAreaSelected?.Invoke(productionArea);
            }
        }

        private void SetState(FarmState newState) => 
            _farmState = newState;
    }
}