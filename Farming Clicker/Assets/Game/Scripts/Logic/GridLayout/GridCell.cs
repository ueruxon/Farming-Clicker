using System;
using Game.Scripts.Logic.Production;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Logic.GridLayout
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private GameObject _outlineVisual;
        [SerializeField] private BoxCollider _collider;
        [Space(4)]
        
        [Header("Debug")]
        [SerializeField] private bool _debugMode;
        [SerializeField] private RectTransform _debugTransform;
        [SerializeField] private TextMeshPro _text;

        private ProductionArea _productionArea;
        
        private Vector3 _currentWorldPosition;
        private Vector2Int _cellPosition;

        public void Init(Vector3 position, Vector2Int cellPosition, float cellSize)
        {
            _currentWorldPosition = position;
            _cellPosition = cellPosition;

            _collider.size = new Vector3(cellSize, 0.5f, cellSize);
            _outlineVisual.transform.localScale = Vector3.one * cellSize;
            _outlineVisual.SetActive(false);

            if (_debugMode)
                DebugText();
        }

        public void AddProductionArea(ProductionArea productionArea)
        {
            _productionArea = productionArea;
        }

        private void OnMouseEnter()
        {
            _outlineVisual.SetActive(true);
        }

        private void OnMouseExit()
        {
            _outlineVisual.SetActive(false);
        }

        private void DebugText()
        {
            //_debugTransform.sizeDelta = new Vector2(_currentPosition.x, _currentPosition.z);
            string position = $"X: {_currentWorldPosition.x}, Z: {_currentWorldPosition.z}";
            _text.SetText(position);
        }
    }
}