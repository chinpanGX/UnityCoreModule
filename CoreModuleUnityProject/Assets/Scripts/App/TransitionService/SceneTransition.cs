using AppCore.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppService.Runtime
{
    public class SceneTransition
    {
        private readonly FadeScreen fadeScreen = ComponentLocator.Get<FadeScreen>();

        public async void ChangeScene(string sceneName)
        {
            await fadeScreen.FadeIn();
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await fadeScreen.FadeOut();
        }
    }
}