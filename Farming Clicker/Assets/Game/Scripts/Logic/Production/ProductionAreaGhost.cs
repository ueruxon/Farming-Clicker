using System;
using UnityEngine;

namespace Game.Scripts.Logic.Production
{
    public class ProductionAreaGhost : MonoBehaviour
    {
        [SerializeField] private Transform _visual;

        private Camera _mainCamera;
        private Vector3 _prevHitPosition;

        private bool _hooked;

        public void Init(float cellSize)
        {
            _visual.localScale = Vector3.one * (cellSize -1f);
            _prevHitPosition = Vector3.zero;
            _hooked = false;
            _mainCamera = Camera.main;
        }

        public void Hook(Transform cell)
        {
            _hooked = true;
            transform.position = cell.position;
        }
        
        public void Unhook(Transform cell)
        {
            _hooked = false;
        }

        private void Update()
        {
            if (_hooked == false)
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                Vector3 groundPosition = _prevHitPosition;
            
                if (Physics.Raycast(ray, out RaycastHit hit, 50f))
                {
                    groundPosition = hit.point;
                    groundPosition.y = 0;
                    _prevHitPosition = groundPosition;
                }

                transform.position = groundPosition;
            }
        }

        public void Destroy() => 
            Destroy(gameObject);
    }
}