using AppCore.Runtime;
using AppService.Runtime;
using UnityEngine;

namespace App.Title
{
    public class TitleDirector : MonoBehaviour, IDirector
    {
        private UpdatablePresenter updatablePresenter;
        private FadeScreen fadeScreen;
        private PresenterFactoryProvider presenterFactoryProvider;
        
        private void Start()
        {
            fadeScreen = ComponentLocator.Get<FadeScreen>();
            updatablePresenter = new UpdatablePresenter();
        }

        public async void Push(string name)
        {
            await fadeScreen.FadeIn();
            var request = await presenterFactoryProvider.Get(name).CreateAsync();
            updatablePresenter.SetRequest(request);
            await fadeScreen.FadeOut(); 
        }
    }

}