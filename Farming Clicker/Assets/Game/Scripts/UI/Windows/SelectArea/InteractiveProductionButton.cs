using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.SelectArea
{
    public enum ProductionButtonType
    {
        Seed,
        Coin
    }
    
    public class InteractiveProductionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _counter;
        [SerializeField] private ProductionButtonType _buttonType;

        public void UpdateCounter(int value) => 
            _counter.SetText(value.ToString());
    }
}