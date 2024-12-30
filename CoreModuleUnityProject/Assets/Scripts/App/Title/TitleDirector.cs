using App.UserService.Application;
using App.UserService.Infrastructure;
using AppCore.Runtime;
using AppService.Runtime;
using UnityEngine;

namespace App.Title
{
    public class TitleDirector : MonoBehaviour, IDirector
    {
        private FadeScreen fadeScreen;
        private PresenterFactoryProvider presenterFactoryProvider;
        private UpdatablePresenter updatablePresenter;

        private void Start()
        {
            if (!ServiceLocator.TryGet<PresenterFactoryProvider>(out var value))
            {
                presenterFactoryProvider = new PresenterFactoryProvider();
                ServiceLocator.Register(presenterFactoryProvider);
            }
            else
            {
                presenterFactoryProvider = value;
            }

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

        public class TitlePresenterFactory : IPresenterFactory
        {
            private readonly IDirector director;
            private readonly ISceneTransitionService sceneTransitionService;

            public TitlePresenterFactory(IDirector director, ISceneTransitionService sceneTransitionService)
            {
                this.director = director;
                this.sceneTransitionService = sceneTransitionService;
            }

            public async Awaitable<IPresenter> CreateAsync()
            {
                var userApplicationService = new UserApplicationService(new PlayerPrefsUserRepository());
                var model = new TitleModel(userApplicationService);
                var presenter = new TitlePresenter(director, model, new TitleView(), sceneTransitionService);
                return presenter;
            }
        }
    }
}