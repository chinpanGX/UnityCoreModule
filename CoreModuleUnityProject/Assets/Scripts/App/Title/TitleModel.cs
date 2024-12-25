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

        public Observable<TransitionState> OnTransitionState => transitionState;

        public TitleModel()
        {
            
        }
        
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