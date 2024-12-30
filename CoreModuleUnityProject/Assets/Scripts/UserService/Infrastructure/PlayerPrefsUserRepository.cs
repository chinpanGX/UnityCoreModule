using System;
using System.Globalization;
using UnityEngine;
using UserData;

namespace App.UserService.Infrastructure
{
    public class PlayerPrefsUserRepository : IUserRepository
    {
        private readonly string createAtKey = "createAt";
        private readonly string idKey = "id";
        private readonly string lastSigninDateTimeKey = "lastSigninDateTime";
        private readonly string nameKey = "name";

        public void Update(UserEntity userEntity)
        {
            PlayerPrefs.SetString(idKey, userEntity.UserId.Value);
            PlayerPrefs.SetString(nameKey, userEntity.Name.Value);
            PlayerPrefs.SetString(createAtKey, userEntity.CreateAt.ToString(CultureInfo.CurrentCulture));
            PlayerPrefs.SetString(lastSigninDateTimeKey,
                userEntity.LastSigninDateTime.ToString(CultureInfo.CurrentCulture)
            );
            PlayerPrefs.Save();
        }

        public bool TryGet(out UserEntity userEntity)
        {
            var id = PlayerPrefs.GetString(idKey, "");
            if (string.IsNullOrEmpty(id))
            {
                userEntity = UserEntity.CreateEmpty();
                return false;
            }
            var name = PlayerPrefs.GetString(nameKey);
            var createAt = PlayerPrefs.GetString(createAtKey);
            var lastSigninDateTime = PlayerPrefs.GetString(lastSigninDateTimeKey);
            userEntity = new UserEntity(UserId.Create(id), UserName.Create(name), DateTime.Parse(createAt),
                DateTime.Parse(lastSigninDateTime)
            );
            return true;
        }
    }
}