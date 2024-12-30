using System;
using UnityEngine;
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
        
        public void Register(string name)
        {
            var userEntity = GetOrCreateEmpty();
            try
            {
                userEntity.UpdateName(UserName.Create(name));
                userRepository.Update(userEntity);
            }   
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        private UserEntity GetOrCreateEmpty()
        {
            return userRepository.TryGet(out var userEntity) ? userEntity : UserEntity.CreateEmpty();
        }
    }
}