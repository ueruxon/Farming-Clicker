using Game.Scripts.Data.Game;
using UnityEngine;

namespace Game.Scripts.Logic.GridLayout
{
    public class GridSystem
    {
        private readonly int _width;
        private readonly int _height;
        private readonly float _cellSize;
        private readonly int _openCellCountByDefault;

        private readonly GridCell _cellPrefab;
        private readonly GridCell[,] _gridArray;
        
        private Transform _gridContainer;

        public GridSystem(GameConfig gameConfig)
        {
            _width = gameConfig.Width;
            _height = gameConfig.Height;
            _cellSize = gameConfig.CellSize;
            _cellPrefab = gameConfig.CellPrefab;
            _openCellCountByDefault = gameConfig.OpenCellByDefault;

            _gridArray = new GridCell[_width, _height];
        }

        public void Init(Transform gridContainer) => 
            _gridContainer = gridContainer;

        public GridCell[,] CreateGrid()
        {
            int openIndex = 0;
            
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    Vector2Int cellPosition = new (x, z);
                    Vector3 worldPosition = GetWorldPosition(cellPosition);
                    //Vector3 worldPosition = GetWorldPosition(cellPosition) + _gridContainer.position;
                    
                    GridCell cell = Object.Instantiate(_cellPrefab, worldPosition, Quaternion.identity);
                    cell.Init(worldPosition, cellPosition, _cellSize);
                    cell.Close();
                    cell.transform.SetParent(_gridContainer);
                    
                    if (openIndex < _openCellCountByDefault)
                        cell.Open();
                    openIndex++;
                    
                    _gridArray[x, z] = cell;
                }
            }

            return _gridArray;
        }

        public GridCell[,] GetGrid() => _gridArray;

        public GridCell GetGridCell(Vector2Int cellPosition) => 
            _gridArray[cellPosition.x, cellPosition.y];

        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / _cellSize);
            int z = Mathf.RoundToInt(worldPosition.z / _cellSize);

            return new Vector2Int(x, z);
        }
        
        public Vector3 GetWorldPosition(Vector2Int cellPosition) => 
            new Vector3(cellPosition.x, 0, cellPosition.y) * _cellSize;
        
        public bool IsValidGridPosition(Vector2Int cellPosition)
        {
            return cellPosition.x >= 0 && 
                   cellPosition.y >= 0 && 
                   cellPosition.x < _width && 
                   cellPosition.y < _height;
        }

        public void SelectAllAvailableCell()
        {
            foreach (GridCell gridCell in _gridArray) 
                gridCell.SetSelectMode(SelectMode.Global);
        }

        public void DeselectAllAvailableCell()
        {
            foreach (GridCell gridCell in _gridArray) 
                gridCell.SetSelectMode(SelectMode.Local);
        }

        public void OpenGridCells(int upgradesCount)
        {
            int openColumnCount = (upgradesCount * _height) + _openCellCountByDefault;
            int openCellCount = 0;

            foreach (GridCell cell in _gridArray)
            {
                if (cell.IsOpen())
                    openCellCount++;
            }
            
            int needOpenCount = openColumnCount - openCellCount;

            foreach (GridCell cell in _gridArray)
            {
                if (needOpenCount > 0 && cell.IsOpen() == false)
                {
                    needOpenCount--;
                    cell.Open();
                }
                
                if (needOpenCount == 0)
                    break;
            }
        }
    }
}