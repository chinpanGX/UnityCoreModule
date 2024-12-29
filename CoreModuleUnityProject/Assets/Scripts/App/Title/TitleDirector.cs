using AppCore.Runtime;
using AppService.Runtime;
using UnityEngine;

namespace App.Title
{
    public class TitleDirector : MonoBehaviour, IDirector
    {
        private UpdatablePresenter updatablePresenter;
        private FadeScreen fadeScreen;
        private IPresenterFactory presenterFactory;
        
        private void Start()
        {
            fadeScreen = ComponentLocator.Get<FadeScreen>();
            updatablePresenter = new UpdatablePresenter();
        }

        public async void Push(string name)
        {
            await fadeScreen.FadeIn();
            
            IPresenter request = name switch
            {
                "Title" => await presenterFactory.CreateAsync(),
                _ => null!
            };
            updatablePresenter.SetRequest(request);
            await fadeScreen.FadeOut(); 
        }
    }

}