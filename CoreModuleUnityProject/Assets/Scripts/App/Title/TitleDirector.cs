using App.UserService.Application;
using App.UserService.Infrastructure;
using AppCore.Runtime;
using AppService.Runtime;
using R3;
using R3.Triggers;
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

            presenterFactoryProvider.TryAdd("Title", new TitlePresenterFactory(this, new SceneTransitionService()));

            fadeScreen = ComponentLocator.Get<FadeScreen>();
            updatablePresenter = new UpdatablePresenter();

            this.UpdateAsObservable().Subscribe(_ => updatablePresenter.Execute()).RegisterTo(destroyCancellationToken);

            Push("Title");
        }

        public async void Push(string name)
        {
            await fadeScreen.FadeIn();
            var request = await presenterFactoryProvider.Get(name).CreateAsync();
            updatablePresenter.SetRequest(request);
            await fadeScreen.FadeOut();
        }

        class TitlePresenterFactory : IPresenterFactory
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
                var view = await TitleView.CreateAsync();
                var presenter = new TitlePresenter(director, model, view, sceneTransitionService);
                return presenter;
            }
        }

        class SignupPresenterFactory : IPresenterFactory
        {
            private readonly IDirector director;
            private readonly ISceneTransitionService sceneTransitionService;

            public SignupPresenterFactory(IDirector director, ISceneTransitionService sceneTransitionService)
            {
                this.director = director;
                this.sceneTransitionService = sceneTransitionService;
            }

            public Awaitable<IPresenter> CreateAsync()
            {
                // var userApplicationService = new UserApplicationService(new PlayerPrefsUserRepository());
                // var model = new SignupModel(userApplicationService);
                // var view = SignupView.Create();
                // var presenter = new SignupPresenter(director, model, view, sceneTransitionService);
                // return presenter;

                return default;
            }
        }
    }
}