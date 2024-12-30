using AppCore.Runtime;
using AppService.Runtime;
using AssetLoader.Application;
using Cysharp.Threading.Tasks;
using R3;

namespace App.Title
{
    public class TitlePresenter : IPresenter
    {
        private IDirector Director { get; set; }
        private TitleModel Model { get; set; }
        private TitleView View { get; set; }
        private FadeScreenView FadeScreenView { get; set; }
        private StateMachine<TitlePresenter> StateMachine { get; set; }
        private ISceneLoader SceneLoader { get; set; }

        public TitlePresenter(IDirector director, TitleModel model, TitleView view, FadeScreenView fadeScreenView,
            ISceneLoader sceneLoader)
        {
            Director = director;
            Model = model;
            View = view;
            FadeScreenView = fadeScreenView;
            SceneLoader = sceneLoader;
            StateMachine = new StateMachine<TitlePresenter>(this);
            StateMachine.Change<StateInit>();
        }

        public void Execute()
        {
            StateMachine.Execute();
        }

        public void Dispose()
        {
            Director = null;
            View.Pop();
            View = null;
            Model.Dispose();
            Model = null;
            StateMachine.Dispose();
            StateMachine = null;
        }

        private class StateInit : StateMachine<TitlePresenter>.State
        {
            private readonly CancellationDisposable cancellationDisposable = new();

            public override void Begin(TitlePresenter owner)
            {
                var view = owner.View;

                view.OnClick.Subscribe(_ => owner.StateMachine.Change<SceneTransitionToHome>())
                    .RegisterTo(cancellationDisposable.Token);

                view.Setup();
                view.Open();
            }

            public override void End(TitlePresenter owner)
            {
                cancellationDisposable.Dispose();
            }
        }

        private class SceneTransitionToHome : StateMachine<TitlePresenter>.State
        {
            public override void Begin(TitlePresenter owner)
            {
                owner.FadeScreenView.Open();
                OnSceneTransitioned(owner).Forget();
            }

            private async UniTask OnSceneTransitioned(TitlePresenter owner)
            {
                await owner.FadeScreenView.FadeIn();
                await owner.SceneLoader.LoadSceneAsync("HomeScene", false);
                await owner.SceneLoader.UnloadSceneAsync("TitleScene");
            }
        }
    }
}