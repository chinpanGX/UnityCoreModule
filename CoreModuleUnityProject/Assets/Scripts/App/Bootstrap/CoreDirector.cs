using App.TransitionService;
using AppService.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.App.Boot
{
    public class CoreDirector : MonoBehaviour
    {
        private async void Start()
        {
            var fade = ComponentLocator.Get<Fade>();
            fade.BlackOut();
            await SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
            fade.FadeOut().Forget();
        }
    }
}