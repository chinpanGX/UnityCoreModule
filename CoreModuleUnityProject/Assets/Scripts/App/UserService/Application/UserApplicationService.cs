using UserData;

namespace App.UserService.Application
{
    public class UserApplicationService
    {
        private readonly IUserRepository userRepository;

        public UserApplicationService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool ExistUserData()
        {
            return userRepository.TryGet(out var userEntity);
        }

        public UserEntity GetOrCreateEmpty()
        {
            return userRepository.TryGet(out var userEntity) ? userEntity : UserEntity.CreateEmpty();
        }

        public void Create(UserEntity userEntity)
        {
            userRepository.Update(userEntity);
        }
    }
}