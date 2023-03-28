using System.Collections;
using Game.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _loadingProgressBar;

        private float _currentProgressValue;
        private float _endProgressValue;
        
        private WaitForSeconds _fadeDelay = new WaitForSeconds(0.03f);

        private void Awake()
        {
            _canvasGroup.SetActive(false);
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            _canvasGroup.SetActive(true);
            _currentProgressValue = 0;
            _endProgressValue = 1;

            StartCoroutine(UpdateLoadingBarRoutine());
        }

        public void Hide() => 
            StartCoroutine(DoFadeInRoutine());

        public void SetProgress(float loadProgress) => 
            _endProgressValue += loadProgress;

        private IEnumerator UpdateLoadingBarRoutine()
        {
            float currentTime = 0;
            while (currentTime < _endProgressValue)
            {
                float t = currentTime / _endProgressValue;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                currentTime += Time.deltaTime;

                float fillAmount = Mathf.Lerp(_currentProgressValue, _endProgressValue, t);
                UpdateProgressBar(fillAmount, _endProgressValue);

                yield return null;
            }
        }

        private void UpdateProgressBar(float value, float maxValue) => 
            _loadingProgressBar.fillAmount = value / maxValue;

        private IEnumerator DoFadeInRoutine()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= 0.03f;
                yield return _fadeDelay;
            }
            
            _canvasGroup.SetActive(false);
        }
    }
}