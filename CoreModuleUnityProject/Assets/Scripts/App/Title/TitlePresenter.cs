using AppCore.Runtime;
using R3;

namespace App.Title
{
    public class TitlePresenter : IPresenter
    {
        private TitleDirector Director { get; set; }
        private TitleModel Model { get; set; }
        private TitleView View { get; set; }
        private StateMachine<TitlePresenter> StateMachine { get; set; }

        public TitlePresenter(TitleDirector director, TitleModel model, TitleView view)
        {
            Director = director;
            Model = model;
            View = view;
            StateMachine = new StateMachine<TitlePresenter>(this);
            StateMachine.Change<StateInit>();
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

        public void Execute()
        {
            StateMachine.Execute();
        }

        class StateInit : StateMachine<TitlePresenter>.State
        {
            readonly CancellationDisposable cancellationDisposable = new ();
            public override void Begin(TitlePresenter owner)
            {
                var model = owner.Model;
                model.OnTransitionState.Subscribe(state => TransitionState(state, owner))
                    .RegisterTo(cancellationDisposable.Token);
                model.Execute();
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

        class StateSignup : StateMachine<TitlePresenter>.State
        {
            public override void Begin(TitlePresenter owner)
            {
                
            }
        }

        class StateHome : StateMachine<TitlePresenter>.State
        {
            public override void Begin(TitlePresenter owner)
            {
                
            }
        }
    }
}