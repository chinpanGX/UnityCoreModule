using App.UserService.Application;
using R3;

namespace App.Home
{
    public class SignupModel
    {
        private readonly UserApplicationService userApplicationService;
        private readonly Subject<Unit> skipSignup = new();
        private readonly Subject<Unit> registerCompleted = new();
        public Observable<Unit> SkipSignup => skipSignup;
        public Observable<Unit> RegisterCompleted => registerCompleted;

        public SignupModel(UserApplicationService userApplicationService)
        {
            this.userApplicationService = userApplicationService;
        }
        
        public bool ExistUserData()
        {
            return userApplicationService.ExistUserData(); 
        }
        
        public void Register(string name)
        {
            userApplicationService.Register(name);
            registerCompleted.OnNext(Unit.Default);
        }
    }
}