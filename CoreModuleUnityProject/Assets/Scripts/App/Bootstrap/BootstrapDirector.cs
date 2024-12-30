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
            var fade = ComponentLocator.Get<FadeScreen>();
            fade.BlackOut();
            Initialize();
            await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
        }

        private void Initialize()
        {
            ServiceLocator.Register(new PresenterFactoryProvider());
        }
    }
}