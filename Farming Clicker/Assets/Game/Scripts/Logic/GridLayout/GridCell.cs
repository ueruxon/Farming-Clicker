using System;
using Game.Scripts.Logic.Production;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Logic.GridLayout
{
    public enum CellState
    {
        Close,
        Open,
        Occupied,
        Selected,
    }
    
    public class GridCell : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Vector2Int> Enter; 
        public event Action<Vector2Int> Exit;
        public event Action<Vector2Int> Clicked; 

        [SerializeField] private GameObject _outlineVisual;
        [SerializeField] private SelectProperties _selectProps;
        [SerializeField] private BoxCollider _collider;
        [Space(4)]
        
        [Header("Debug")]
        [SerializeField] private bool _debugMode;
        [SerializeField] private RectTransform _debugTransform;
        [SerializeField] private TextMeshPro _text;

        private ProductionArea _productionArea;

        public CellState _cellState;
        public SelectMode _selectMode;

        private Vector3 _currentWorldPosition;
        private Vector2Int _cellPosition;

        public void Init(Vector3 position, Vector2Int cellPosition, float cellSize)
        {
            _currentWorldPosition = position;
            _cellPosition = cellPosition;

            _collider.size = new Vector3(cellSize, 0.5f, cellSize);
            
            _selectProps.OutlineVisual.transform.localScale = Vector3.one * cellSize;
            _selectProps.SpriteRenderer.color = _selectProps.DefaultColor;
            _selectProps.OutlineVisual.SetActive(false);
            //_outlineVisual.transform.localScale = Vector3.one * cellSize;
            //_outlineVisual.SetActive(false);
            
            if (_debugMode)
                DebugText();

            SetSelectMode(SelectMode.Local);
        }

        public void SetSelectMode(SelectMode selectMode)
        {
            _selectMode = selectMode;
            //_outlineVisual.SetActive(selectMode == SelectMode.Global && IsAvailable());
            _selectProps.OutlineVisual.SetActive(selectMode == SelectMode.Global && IsAvailable());
        }

        public void Open() => 
            _cellState = CellState.Open;

        public void Close() => 
            _cellState = CellState.Close;

        public bool IsAvailable() => 
            _cellState == CellState.Open;

        public void Select(bool select)
        {
            if (select)
            {
                _cellState = CellState.Selected;
                //_outlineVisual.SetActive(true);
                _selectProps.SpriteRenderer.color = _selectProps.SelectedColor;
                _selectProps.OutlineVisual.SetActive(true);
            }
            else
            {
                _cellState = CellState.Occupied;
                //_outlineVisual.SetActive(false);
                _selectProps.SpriteRenderer.color = _selectProps.DefaultColor;
                _selectProps.OutlineVisual.SetActive(false);
            }
        }

        public void AddProductionArea(ProductionArea productionArea)
        {
            _productionArea = productionArea;
            _cellState = CellState.Occupied;
        }

        public void ClearProductionArea()
        {
            _productionArea = null;
            _cellState = CellState.Open;
        }

        public ProductionArea GetProductionArea() => 
            _productionArea;

        private void OnMouseEnter()
        {
            if (_cellState == CellState.Occupied
                && _selectMode == SelectMode.Local)
            {
                //_outlineVisual.SetActive(true);
                _selectProps.OutlineVisual.SetActive(true);
            }

            if (_cellState == CellState.Open && _selectMode == SelectMode.Global) 
                Enter?.Invoke(_cellPosition);
        }

        private void OnMouseExit()
        {
            if (_cellState == CellState.Occupied
                && _selectMode == SelectMode.Local)
            {
                //_outlineVisual.SetActive(false);
                _selectProps.OutlineVisual.SetActive(false);
            }
            
            if (_cellState == CellState.Open && _selectMode == SelectMode.Global) 
                Exit?.Invoke(_cellPosition);
        }

        private void DebugText()
        {
            //_debugTransform.sizeDelta = new Vector2(_currentPosition.x, _currentPosition.z);
            string position = $"X: {_currentWorldPosition.x}, Z: {_currentWorldPosition.z}";
            _text.SetText(position);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if ((_cellState == CellState.Occupied && _selectMode == SelectMode.Local) 
                || (_cellState == CellState.Open && _selectMode == SelectMode.Global))
            {
                Clicked?.Invoke(_cellPosition);
            }
        }
    }

    [Serializable]
    public class SelectProperties
    {
       public GameObject OutlineVisual;
       public SpriteRenderer SpriteRenderer;
       public Color DefaultColor;
       public Color SelectedColor;
    }
}