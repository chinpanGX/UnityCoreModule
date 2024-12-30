using System.Threading;
using AppCore.Runtime;
using AppService.Runtime;
using AssetLoader.Application;
using R3;
using UnityEngine;

namespace App.Home
{
    public class SignupPresenter : IPresenter
    {
        private IDirector Director { get; set; }
        private SignupModel Model { get; set; }
        private SignupView View { get; set; }
        private FadeScreenView FadeScreenView { get; set; }
        private IAssetLoader AssetLoader { get; set; }
        private StateMachine<SignupPresenter> StateMachine { get; set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }

        public SignupPresenter(IDirector director, SignupModel model, SignupView view, FadeScreenView fadeScreenView, IAssetLoader assetLoader)
        {
            Director = director;
            Model = model;
            View = view;
            FadeScreenView = fadeScreenView;
            AssetLoader = assetLoader;
            CancellationTokenSource = new CancellationTokenSource();
            StateMachine = new StateMachine<SignupPresenter>(this);
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
            Model = null;
            StateMachine.Dispose();
            StateMachine = null;
            FadeScreenView = null;
            AssetLoader = null;
        }

        private class StateInit : StateMachine<SignupPresenter>.State
        {
            public override async void Begin(SignupPresenter owner)
            {
                var view = owner.View;
                var model = owner.Model;

                if (model.ExistUserData())
                {
                    owner.StateMachine.Change<StateTransitionToHome>();
                    return;
                }
                
                owner.FadeScreenView.Push();
                owner.FadeScreenView.Open();
                owner.FadeScreenView.BlackOut();

                view.Push();
                view.Open();
                
                owner.StateMachine.Change<StateSignup>();
            }
        }

        private class StateSignup : StateMachine<SignupPresenter>.State
        {
            public override async void Begin(SignupPresenter owner)
            {
                var view = owner.View;
                var model = owner.Model;

                model.RegisterCompleted
                    .Subscribe(_ => owner.StateMachine.Change<StateTransitionToHome>())
                    .RegisterTo(owner.CancellationTokenSource.Token);

                view.RegisterRequestUserName
                    .Subscribe((name) => owner.Model.Register(name))
                    .RegisterTo(owner.CancellationTokenSource.Token);

                await owner.FadeScreenView.FadeOut();
            }
        }

        private class StateTransitionToHome : StateMachine<SignupPresenter>.State
        {
            public override void Begin(SignupPresenter owner)
            {
                owner.Director.Push("Home");
            }
        }
    }
}