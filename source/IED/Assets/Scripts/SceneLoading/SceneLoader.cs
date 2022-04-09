using System;
using System.Collections;
using Services;
using UnityEngine.SceneManagement;

namespace SceneLoading
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtain _loadingCurtain;

        public SceneLoader(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _coroutineRunner = coroutineRunner;
            _loadingCurtain = loadingCurtain;
        }

        public void Load(string name, Action onLoaded = null, Action onCurtainHide = null)
        {
            _coroutineRunner.StopAllCoroutines();
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded, onCurtainHide));
        }

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null, Action onCurtainHide = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            _loadingCurtain.Show();
            while (_loadingCurtain.IsShown == false)
                yield return null;

            var waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (waitNextScene.isDone == false)
                yield return null;

            _loadingCurtain.Hide();

            onLoaded?.Invoke();
            while (_loadingCurtain.IsShown)
                yield return null;

            onCurtainHide?.Invoke();
        }
    }
}