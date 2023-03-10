using UnityEngine;

namespace Game.Scripts.Logic.Production
{
    public class ProductionVisual : MonoBehaviour
    {
        private MeshRenderer[] _productionViewRenderers;
        private Transform[] _productionViewArray;
        
        private float _maxHeight = 1f;

        public void Init(Material viewMaterial)
        {
            _productionViewRenderers = GetComponentsInChildren<MeshRenderer>();
            _productionViewArray = GetComponentsInChildren<Transform>();

            foreach (MeshRenderer meshRenderer in _productionViewRenderers) 
                meshRenderer.sharedMaterial = viewMaterial;

            foreach (Transform view in _productionViewArray)
            {
                Vector3 defaultScale = new Vector3(view.localScale.x, 0, view.localScale.z);
                view.localScale = defaultScale;
            }
        }

        public void UpdateVisual(float elapsedTime, float lerpDuration)
        {
            float height = Mathf.Lerp(0, _maxHeight, elapsedTime / lerpDuration);
            
            foreach (Transform view in _productionViewArray)
                view.localScale = new Vector3(view.localScale.x, height, view.localScale.z);

        }
    }
}