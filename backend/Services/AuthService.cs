using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Accounts;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthManager _authManager;

        public AuthService(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        public async Task<NewAppUserResponseDto> RegisterUserAsync(RegisterAccountDto dto)
        {
            try
            {
                var appUser = new AppUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.Username,
                    Email = dto.Email,
                };

                var (createdUser, token) = await _authManager.RegisterNewUserAsync(appUser, dto.Password);

                return new NewAppUserResponseDto
                {
                    Success = true,
                    Data = new NewAccountDto
                    {
                        Username = createdUser.UserName,
                        Email = createdUser.Email,
                        Token = token
                    }
                };
            }
            catch (Exception e)
            {
                return new NewAppUserResponseDto { Success = false, Errors = new[] { e.Message } };
            }
        }

        public async Task<NewAppUserResponseDto> LoginUserAsync(LoginAccountDto dto)
        {
            try
            {
                var (user, token) = await _authManager.SignInUserAsync(dto.Identifier, dto.Password);

                return new NewAppUserResponseDto
                {
                    Success = true,
                    Data = new NewAccountDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = token
                    }
                };
            }
            catch (Exception e)
            {
                return new NewAppUserResponseDto { Success = false, Errors = new[] { e.Message } };
            }
        }
    }
}