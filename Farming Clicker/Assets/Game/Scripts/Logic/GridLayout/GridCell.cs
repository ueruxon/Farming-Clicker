using System;
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

        private Vector3 _currentPosition;

        public void Init(Vector3 position, float cellSize)
        {
            _currentPosition = position;

            _collider.size = new Vector3(cellSize, 0.5f, cellSize);
            _outlineVisual.transform.localScale = Vector3.one * cellSize;
            _outlineVisual.SetActive(false);

            if (_debugMode)
                DebugText();
        }

        private void DebugText()
        {
            //_debugTransform.sizeDelta = new Vector2(_currentPosition.x, _currentPosition.z);
            string position = $"X: {_currentPosition.x}, Z: {_currentPosition.z}";
            _text.SetText(position);
        }

        private void OnMouseEnter()
        {
            _outlineVisual.SetActive(true);
        }
        
        private void OnMouseExit()
        {
            _outlineVisual.SetActive(false);
        }
    }
}