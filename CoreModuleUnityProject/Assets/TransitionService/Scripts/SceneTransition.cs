using AppService.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TransitionService
{
    public static class SceneName
    {
        public static readonly string Core = "CoreScene";
        public static readonly string Title = "TitleScene";
    }

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