using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Infrastructure.Setup
{
    public class SceneLoader
    {
        private readonly LoadingCurtain _loadingCurtain;

        public SceneLoader(LoadingCurtain loadingCurtain) => 
            _loadingCurtain = loadingCurtain;

        public async UniTask Load(string sceneName, Action onLoadedCallback = null)
        {
            _loadingCurtain.Show();
            await LoadScene(sceneName, onLoadedCallback);
            await UniTask.Delay(2000);
            _loadingCurtain.Hide();
        }

        private async UniTask LoadScene(string nextScene, Action onLoadedCallback = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoadedCallback?.Invoke();
                await UniTask.Yield();
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
            {
                _loadingCurtain.SetProgress(waitNextScene.progress);
                await UniTask.Yield();
            }

            onLoadedCallback?.Invoke();
        }
    }
}