using System;

namespace UserData
{
    public class UserEntity
    {

        public UserEntity(UserId userId, UserName name, DateTime createAt, DateTime lastSigninDateTime)
        {
            UserId = userId;
            Name = name;
            CreateAt = createAt;
            LastSigninDateTime = lastSigninDateTime;
        }

        public UserId UserId { get; }
        public UserName Name { get; }
        public DateTime CreateAt { get; }
        public DateTime LastSigninDateTime { get; }
        public static UserEntity CreateNew(UserName userName)
        {
            var userId = UserId.New();
            return new UserEntity(userId, userName, DateTime.Now, DateTime.Now);
        }

        public static UserEntity CreateEmpty()
        {
            return new UserEntity(UserId.New(), UserName.DefaultValue, DateTime.MinValue, DateTime.MinValue);
        }
    }
}