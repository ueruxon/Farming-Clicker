using UnityEngine;

namespace Game.Scripts.Logic.GridLayout
{
    public class GridSystem
    {
        private readonly int _width;
        private readonly int _height;
        private readonly float _cellSize;
        
        private readonly GridCell _cellPrefab;
        private readonly Transform _gridContainer;

        private GridCell[,] _gridArray;

        public GridSystem(int width, int height, float cellSize, GridCell cellPrefab, Transform gridContainer)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _cellPrefab = cellPrefab;
            _gridContainer = gridContainer;

            _gridArray = new GridCell[_width, _height];
        }

        public GridCell[,] CreateGrid()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    Vector2Int cellPosition = new (x, z);
                    Vector3 worldPosition = GetWorldPosition(cellPosition);
                    //Vector3 worldPosition = GetWorldPosition(cellPosition) + _gridContainer.position;
                    
                    GridCell cell = Object.Instantiate(_cellPrefab, worldPosition, Quaternion.identity);
                    cell.Init(worldPosition, cellPosition, _cellSize);
                    cell.transform.SetParent(_gridContainer);

                    _gridArray[x, z] = cell;
                }
            }

            return _gridArray;
        }

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
    }
}