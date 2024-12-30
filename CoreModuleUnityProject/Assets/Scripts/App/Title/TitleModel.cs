using App.UserService.Application;
using R3;

namespace App.Title
{
    public class TitleModel
    {
        public enum TransitionState
        {
            Signup,
            Home
        }

        private readonly Subject<TransitionState> transitionState = new();

        public TitleModel(UserApplicationService userApplicationService)
        {
            if (userApplicationService.ExistUserData())
            {
                transitionState.OnNext(TransitionState.Home);
            }
        }

        public Observable<TransitionState> OnTransitionState => transitionState;

        public void Dispose()
        {
            transitionState.OnCompleted();
            transitionState.Dispose();
        }

        public void Execute()
        {

        }
    }
}