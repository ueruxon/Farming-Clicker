using System.Collections;
using Game.Scripts.Common;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Logic.Cameras
{
    public class CameraController
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly FarmController _farmController;

        private Camera _camera;
        private Vector3 _defaultPosition;
        private float _xLimit = 40;

        public CameraController(ICoroutineRunner coroutineRunner, FarmController farmController)
        {
            _coroutineRunner = coroutineRunner;
            _farmController = farmController;
            _farmController.GridCellSelected += OnProductionAreaSelected;
            _farmController.GridCellDeselected += OnProductionAreaDeselected;
        }

        public void Init()
        {
            _camera = Camera.main;
            _defaultPosition = _camera.transform.position;
        }

        private void OnProductionAreaSelected(GridCell cell)
        {
            var area = cell.GetProductionArea();
            
            float xPosition = Mathf.Clamp(
                (area.transform.position.x / 2f) + _defaultPosition.x, 
                _defaultPosition.x, 
                _xLimit);
            Vector3 endPosition = _defaultPosition;
            endPosition.x = xPosition;

            float different = Mathf.Abs(_camera.transform.position.x - xPosition);
            if (different > 8)
            {
                _coroutineRunner.StopAllCoroutines();
                _coroutineRunner.StartCoroutine(FollowToTargetRoutine(_camera.transform.position, endPosition));
            }
        }

        private void OnProductionAreaDeselected()
        {
            _coroutineRunner.StopAllCoroutines();
            _coroutineRunner.StartCoroutine(FollowToTargetRoutine(_camera.transform.position, _defaultPosition));
        }

        private IEnumerator FollowToTargetRoutine(Vector3 startPosition, Vector3 endPosition)
        {
            float lerpDuration = .5f;
            float elapsedTime = 0;

            while (elapsedTime < lerpDuration)
            {
                elapsedTime += Time.deltaTime;
                Vector3 nextPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / lerpDuration);
                _camera.transform.position = nextPosition;
                
                yield return null;
            }

            _camera.transform.position = endPosition;
        }

        public void Cleanup()
        {
            _farmController.GridCellSelected -= OnProductionAreaSelected;
            _farmController.GridCellDeselected -= OnProductionAreaDeselected;
        }
    }
}