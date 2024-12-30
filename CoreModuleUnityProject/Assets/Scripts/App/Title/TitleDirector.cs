using AppCore.Runtime;
using AppService.Runtime;
using AssetLoader.Infrastructure;
using R3;
using R3.Triggers;
using UnityEngine;

namespace App.Title
{
    public class TitleDirector : MonoBehaviour, IDirector
    {
        [SerializeField] private TitleView titleView;
        [SerializeField] private FadeScreenView fadeScreenView;
        private PresenterFactoryProvider presenterFactoryProvider;
        private UpdatablePresenter updatablePresenter;

        private async void Start()
        {
            titleView.Push();
            titleView.Close();
            fadeScreenView.Push();
            fadeScreenView.Open();
            fadeScreenView.BlackOut();
            
            if (!ServiceLocator.TryGet<PresenterFactoryProvider>(out var value))
            {
                presenterFactoryProvider = new PresenterFactoryProvider();
                ServiceLocator.Register(presenterFactoryProvider);
            }
            else
            {
                presenterFactoryProvider = value;
            }

            var request = new TitlePresenter(this, new TitleModel(), titleView, fadeScreenView, new SceneLoader());
            
            updatablePresenter = new UpdatablePresenter();
            updatablePresenter.SetRequest(request);
            this.UpdateAsObservable().Subscribe(_ => updatablePresenter.Execute()).RegisterTo(destroyCancellationToken);

            await fadeScreenView.FadeOut();
        }

        public void Push(string name)
        {
               
        }
    }
}