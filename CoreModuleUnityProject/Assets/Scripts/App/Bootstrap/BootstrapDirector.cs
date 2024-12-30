using AppCore.Runtime;
using AppService.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.App.Boot
{
    public class BootstrapDirector : MonoBehaviour
    {
        private async void Start()
        {
            Initialize();
            var fade = ComponentLocator.Get<FadeScreen>();
            fade.BlackOut();
            await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
            await fade.FadeOut();
        }

        private void Initialize()
        {
            ServiceLocator.Register(new PresenterFactoryProvider());
        }
    }
}