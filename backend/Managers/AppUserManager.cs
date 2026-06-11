using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;

namespace backend.Managers
{
    public class AppUserManager : IAppUserManager
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserManager(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var user = await _appUserRepository.GetUserByEmailAsync(email);
            if(user == null)
                throw new Exception("User not found");

            return user;
        }
        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            var users = await _appUserRepository.GetAllUsersAsync();
            if(users == null)
                throw new Exception("No users in database");

            return users;
        }
    }
}