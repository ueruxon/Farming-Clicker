using System;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure.Services.Factory;
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
        public event Action ProductionBuilt;
        public event Action<ProductionArea> ProductAreaSelected;
        public event Action ProductAreaDeselected;

        private readonly GameFactory _gameFactory;
        private readonly GridSystem _gridSystem;

        private Camera _mainCamera;
        
        private ProductionAreaGhost _areaGhost;
        private ProductType _activeProductType;

        private FarmState _farmState;

        public FarmController(GameFactory gameFactory, GridSystem gridSystem)
        {
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

        public void CreateGhostArea(ProductType productType)
        {
            SetState(FarmState.Build);
            _activeProductType = productType;
            
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = _mainCamera.nearClipPlane;
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(mousePosition);
            
            _areaGhost = _gameFactory.CreateAreaGhost(worldPoint);
            _gridSystem.SelectAllAvailableCell();
        }

        public void DeselectArea()
        {
            SetState(FarmState.Select);
            ProductAreaDeselected?.Invoke();
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
                _areaGhost.Destroy();
                _areaGhost = null;
                _gridSystem.DeselectAllAvailableCell();
            
                CreateProductionArea(_activeProductType, cellPosition);
                SetState(FarmState.Select);
                
                _activeProductType = ProductType.None;
                
                ProductionBuilt?.Invoke();
                
                return;
            }

            if (_farmState == FarmState.Select)
            {
                GridCell cell = _gridSystem.GetGridCell(cellPosition);
                ProductionArea productionArea = cell.GetProductionArea();
                
                ProductAreaSelected?.Invoke(productionArea);
            }
        }

        private void SetState(FarmState newState) => 
            _farmState = newState;
    }
}