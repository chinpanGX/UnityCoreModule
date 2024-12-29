namespace UserData
{
    public interface IUserRepository
    {
        void Update(UserEntity userEntity);
        UserEntity Get();
    }
}