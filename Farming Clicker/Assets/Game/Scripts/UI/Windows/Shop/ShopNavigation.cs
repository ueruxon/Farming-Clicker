using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Windows.Shop
{
    public enum NavigationMode
    {
        Forward,
        Backward
    }
    
    public class ShopNavigation : MonoBehaviour
    {
        public event Action<NavigationMode, int> ContentIndexChanged; 

        [SerializeField] private Button _forwardButton;
        [SerializeField] private Button _backwardButton;

        private int _contentCount;
        private int _activeContentIndex;

        private int ActiveContentIndex
        {
            set => _activeContentIndex = Mathf.Clamp(value, 0, _contentCount);
            get => _activeContentIndex;
        }

        public void Init(int contentCount, int activeContentIndex)
        {
            _contentCount = contentCount - 1;
            _activeContentIndex = activeContentIndex;
            
            _forwardButton.onClick.AddListener(OnForward);
            _backwardButton.onClick.AddListener(OnBackward);

            UpdateView();
        }

        private void UpdateView()
        {
            _forwardButton.gameObject.SetActive(true);
            _backwardButton.gameObject.SetActive(true);

            if (ActiveContentIndex == 0 && _contentCount > 0)
            {
                _forwardButton.gameObject.SetActive(true);
                _backwardButton.gameObject.SetActive(false);
            }

            if (ActiveContentIndex == _contentCount)
            {
                _forwardButton.gameObject.SetActive(false);
                _backwardButton.gameObject.SetActive(true);
            }
        }

        private void OnForward()
        {
            ActiveContentIndex++;
            UpdateView();
            
            ContentIndexChanged?.Invoke(NavigationMode.Forward, ActiveContentIndex);
        }

        private void OnBackward()
        {
            ActiveContentIndex--;
            UpdateView();
            
            ContentIndexChanged?.Invoke(NavigationMode.Backward, ActiveContentIndex);
        }

        private void OnDestroy()
        {
            _forwardButton.onClick.RemoveListener(OnForward);
            _backwardButton.onClick.RemoveListener(OnBackward);
        }
    }
}