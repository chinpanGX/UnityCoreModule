using System.Globalization;
using UnityEngine;
using UserData;

namespace App.UserService.Infrastructure
{
    public class PlayerPrefsUserRepository : IUserRepository
    {
        private readonly string nameKey = "name";
        private readonly string idKey = "id";
        private readonly string createAtKey = "createAt";
        private readonly string lastSigninDateTimeKey = "lastSigninDateTime";
        
        public void Update(UserEntity userEntity)
        {
            PlayerPrefs.SetString(idKey, userEntity.UserId.Value);
            PlayerPrefs.SetString(nameKey, userEntity.Name.Value);
            PlayerPrefs.SetString(createAtKey, userEntity.CreateAt.ToString(CultureInfo.CurrentCulture));
            PlayerPrefs.SetString(lastSigninDateTimeKey, userEntity.LastSigninDateTime.ToString(CultureInfo.CurrentCulture));
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
            else
            {
                var name = PlayerPrefs.GetString(nameKey);
                var createAt = PlayerPrefs.GetString(createAtKey);
                var lastSigninDateTime = PlayerPrefs.GetString(lastSigninDateTimeKey);
                userEntity = new UserEntity(UserId.Create(id), UserName.Create(name), System.DateTime.Parse(createAt),
                    System.DateTime.Parse(lastSigninDateTime)
                );
                return true;   
            }
        }
    }
}
