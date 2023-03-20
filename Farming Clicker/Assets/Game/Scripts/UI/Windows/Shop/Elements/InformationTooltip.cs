using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Windows.Shop.Elements
{
    public class InformationTooltip : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;

        public void ShowInformation(string title, string description)
        {
            _title.SetText(title);
            _description.SetText(description);
            
            _canvasGroup.alpha = 1;
        }

        public void Hide() => 
            _canvasGroup.alpha = 0;
    }
}