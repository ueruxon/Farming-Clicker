using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.HUD.Elements
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Outline _outline;

        private void Awake() => 
            _outline.enabled = false;

        public void OnPointerEnter(PointerEventData eventData) => 
            _outline.enabled = true;

        public void OnPointerExit(PointerEventData eventData) => 
            _outline.enabled = false;

        public void OnDisable() => 
            _outline.enabled = false;
    }
}