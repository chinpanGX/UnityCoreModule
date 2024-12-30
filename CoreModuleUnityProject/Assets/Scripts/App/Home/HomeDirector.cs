# nullable enable
using App.UserService.Application;
using App.UserService.Infrastructure;
using AppCore.Runtime;
using AppService.Runtime;
using AssetLoader.Application;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using R3.Triggers;

namespace App.Home
{
    public class HomeDirector : MonoBehaviour, IDirector
    {
        [SerializeField] private FadeScreenView? fadeScreenView;

        private PresenterFactoryProvider presenterFactoryProvider;
        private UpdatablePresenter updatablePresenter;
        private IAssetLoader assetLoader;

        void Start()
        {
            if (fadeScreenView == null)
            {
                Debug.LogError("FadeScreenView is null");
                return;
            }
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

            assetLoader = new AssetLoader.Infrastructure.AssetLoader();

            presenterFactoryProvider.TryAdd("Signup",
                new SignupPresenterFactory(this, assetLoader, fadeScreenView)
            );

            updatablePresenter = new UpdatablePresenter();
            this.UpdateAsObservable()
                .Subscribe(_ => updatablePresenter.Execute())
                .RegisterTo(destroyCancellationToken);

            Push("Signup");
        }
        
        void OnDestroy()
        {
            assetLoader.Dispose();
        }

        public async void Push(string name)
        {
            var request = await presenterFactoryProvider.Get(name).CreateAsync();
            updatablePresenter.SetRequest(request);
        }
        
        class SignupPresenterFactory : IPresenterFactory
        {
            private readonly IDirector director;
            private readonly IAssetLoader assetLoader;
            private readonly FadeScreenView fadeScreenView;

            public SignupPresenterFactory(IDirector director, IAssetLoader assetLoader, FadeScreenView fadeScreenView)
            {
                this.director = director;
                this.assetLoader = assetLoader;
                this.fadeScreenView = fadeScreenView;
            }

            public async Awaitable<IPresenter> CreateAsync()
            {
                var userApplicationService = new UserApplicationService(new PlayerPrefsUserRepository());
                var model = new SignupModel(userApplicationService);
                var view = await SignupView.CreateAsync(assetLoader);
                var presenter = new SignupPresenter(director, model, view, fadeScreenView, assetLoader);
                return presenter;
            }
        }
    }
}