using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Migrations;
using backend.Models;
using backend.Interfaces;

namespace backend.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly IAppUserRepository _repository;
        private readonly ITokenService _tokenService;

        public AuthManager(IAppUserRepository repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public async Task<(AppUser, string)> RegisterNewUserAsync(AppUser appUser, string password)
        {
            var existed = await _repository.GetUserByEmailAsync(appUser.Email);
            if(existed != null)
                throw new Exception("Email already existed");

            var created = await _repository.CreateUserAsync(appUser, password);
            if(!created.Succeeded)
                throw new Exception(created.Errors.FirstOrDefault().Description ?? "Registration Failed");

            var roleAdded = await _repository.AddUserRoleAsync(appUser, "User");
            if(!roleAdded.Succeeded)
                throw new Exception(roleAdded.Errors.FirstOrDefault().Description ?? "Registration Failed");

            var token = _tokenService.CreateToken(appUser);

            return (appUser, token);
        }

        public async Task<(AppUser, string)> SignInUserByEmailAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmailAsync(email);

            if(user == null)
                throw new Exception("Email doesn't exist");

            var result = await _repository.CheckSignInPassword(user, password);

            if(result.IsLockedOut)
                throw new Exception("Account is locked due to multiple failed login attempts. Please try again in 5 minutes.");

            if(!result.Succeeded)
                throw new Exception("Email not found and/or incorrect password");

            var token = _tokenService.CreateToken(user);
            return (user, token);
        }

        public async Task<(AppUser, string)> SignInUserByUsernameAsync(string username, string password)
        {
            var user = await _repository.GetUserByUsernameAsync(username);

            if(user == null)
                throw new Exception("Username doesn't exist");

            var result = await _repository.CheckSignInPassword(user, password);

            if(result.IsLockedOut)
                throw new Exception("Account is locked due to multiple failed login attempts. Please try again in 5 minutes.");

            if(!result.Succeeded)
                throw new Exception("Email not found and/or incorrect password");

            var token = _tokenService.CreateToken(user);
            return (user, token);
        }
    }
}