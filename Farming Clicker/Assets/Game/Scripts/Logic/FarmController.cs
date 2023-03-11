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
        private readonly GameFactory _gameFactory;
        private readonly GridSystem _gridSystem;
        private readonly GameConfig _gameConfig;

        private Camera _mainCamera;
        private ProductionAreaGhost _areaGhost;

        private FarmState _farmState;

        public FarmController(GameFactory gameFactory, GridSystem gridSystem, GameConfig gameConfig)
        {
            _gameFactory = gameFactory;
            _gridSystem = gridSystem;
            _gameConfig = gameConfig;
            
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
            
            
            // для теста
            CreateProductionArea(ProductType.Wheat, new Vector2Int(0, 0));
            CreateProductionArea(ProductType.Wheat, new Vector2Int(0, 1));
            CreateProductionArea(ProductType.Wheat, new Vector2Int(0, 2));
            
            // CreateProductionArea(ProductType.Potato, new Vector2Int(1, 0));
            // CreateProductionArea(ProductType.Tomato, new Vector2Int(1, 1));
            //CreateProductionArea(ProductType.Pumpkin, new Vector2Int(5, 2));
            
            CreateGhostArea();
        }

        public void CreateGhostArea()
        {
            SetState(FarmState.Build);
            
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = _mainCamera.nearClipPlane;
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(mousePosition);
            
            _areaGhost = _gameFactory.CreateAreaGhost(worldPoint);
            _gridSystem.SelectAllAvailableCell();
        }

        private void CreateProductionArea(ProductType productType, Vector2Int cellPosition)
        {
            GridCell gridCell = _gridSystem.GetGridCell(cellPosition);
            Vector3 spawnPosition = _gridSystem.GetWorldPosition(cellPosition);
            ProductionArea productionArea = _gameFactory.CreateProductionArea(productType, at: spawnPosition);
            gridCell.AddProductionArea(productionArea);
            
            // для теста
            productionArea.ActivateProduction();
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
            Debug.Log("click");
            
            if (_farmState == FarmState.Build)
            {
                _areaGhost.Destroy();
                _areaGhost = null;
                _gridSystem.DeselectAllAvailableCell();
            
                CreateProductionArea(ProductType.Tomato, cellPosition);
                SetState(FarmState.Select);
                
                return;
            }

            // if (_farmState == FarmState.Select)
            // {
            //     GridCell cell = _gridSystem.GetGridCell(cellPosition);
            //     
            //     // может вынести в скрипт камеры
            //     Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            //     
            //     if (Physics.Raycast(ray, out RaycastHit hit, 50f))
            //     {
            //         Vector3 difference = hit.point - _mainCamera.transform.position;
            //         Vector3 scaledDelta = difference * 0.5f;
            //
            //         Vector3 delta = difference - scaledDelta;
            //         
            //         Debug.Log($"scaledDelta: {scaledDelta}");
            //         Debug.Log($"delta: {delta}");
            //
            //         // Vector3 zoomOffset = new Vector3(
            //         //     _mainCamera.transform.position.x + hit.point.x / 2,
            //         //     _mainCamera.transform.position.y + hit.point.z / 2,
            //         //     _mainCamera.transform.position.z);
            //         //
            //         // _mainCamera.transform.position = zoomOffset;
            //         // _mainCamera.orthographicSize = 8f;
            //     }
            //     
            //     //_mainCamera.transform.LookAt(cell.transform);
            // }
        }

        private void SetState(FarmState newState) => 
            _farmState = newState;
    }
}