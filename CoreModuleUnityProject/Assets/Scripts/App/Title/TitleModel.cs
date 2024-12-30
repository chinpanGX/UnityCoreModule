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
        private readonly UserApplicationService userApplicationService;
        
        public Observable<TransitionState> OnTransitionState => transitionState;

        public TitleModel(UserApplicationService userApplicationService)
        {
            this.userApplicationService = userApplicationService;
        }

        public void ChangeTransitionState()
        {
            if (userApplicationService.ExistUserData())
            {
                transitionState.OnNext(TransitionState.Home);
            }
        }

        public void Dispose()
        {
            transitionState.OnCompleted();
            transitionState.Dispose();
        }
    }
}