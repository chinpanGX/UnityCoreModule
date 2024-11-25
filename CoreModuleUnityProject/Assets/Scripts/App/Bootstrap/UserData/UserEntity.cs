using System;
using AppCore.Runtime.UserData;

namespace AppService.Runtime.UserData
{
    public class UserEntity
    {

        // TODO : 保存したデータから復元する

        private UserEntity(UserId userId, UserName name, DateTime createAt, DateTime lastSigninDateTime)
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
    }
}