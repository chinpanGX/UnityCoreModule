namespace UserData
{
    public interface IUserRepository
    {
        void Update(UserEntity userEntity);
        bool TryGet(out UserEntity userEntity);
    }
}