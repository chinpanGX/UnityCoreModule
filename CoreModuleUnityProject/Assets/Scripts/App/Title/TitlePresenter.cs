using AppCore.Runtime;
using R3;

namespace App.Title
{
    public class TitlePresenter : IPresenter
    {
        private IDirector Director { get; set; }
        private TitleModel Model { get; set; }
        private TitleView View { get; set; }
        private StateMachine<TitlePresenter> StateMachine { get; set; }
        private ISceneTransitionService SceneTransitionService { get; }

        public TitlePresenter(IDirector director, TitleModel model, TitleView view,
            ISceneTransitionService sceneTransitionService)
        {
            Director = director;
            Model = model;
            View = view;
            SceneTransitionService = sceneTransitionService;
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
                var model = owner.Model;
                var view = owner.View;

                view.OnClick.Subscribe(_ => model.ChangeTransitionState())
                    .RegisterTo(cancellationDisposable.Token);

                model.OnTransitionState.Subscribe(state => TransitionState(state, owner))
                    .RegisterTo(cancellationDisposable.Token);

                view.Setup();
                view.Push();
                view.Open();
            }

            public override void End(TitlePresenter owner)
            {
                cancellationDisposable.Dispose();
            }

            private void TransitionState(TitleModel.TransitionState state, TitlePresenter owner)
            {
                switch (state)
                {
                    case TitleModel.TransitionState.Signup:
                        owner.StateMachine.Change<StateSignup>();
                        break;
                    case TitleModel.TransitionState.Home:
                        owner.StateMachine.Change<StateHome>();
                        break;
                }
            }
        }

        private class StateSignup : StateMachine<TitlePresenter>.State
        {
            public override void Begin(TitlePresenter owner)
            {
                owner.Director.Push("Signup");
            }
        }

        private class StateHome : StateMachine<TitlePresenter>.State
        {
            public override void Begin(TitlePresenter owner)
            {
                owner.SceneTransitionService.ChangeScene("Home");
            }
        }
    }
}