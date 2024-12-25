using AppService.Runtime;
using TransitionService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.App.Boot
{
    public class BootstrapDirector : MonoBehaviour
    {
        private async void Start()
        {
            var fade = ComponentLocator.Get<FadeScreen>();
            fade.BlackOut();
            await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
            await fade.FadeOut();
        }
    }
}