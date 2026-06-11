using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Dtos.Accounts;
using backend.Models;

namespace backend.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserManager _appUserManager;

        public AppUserService(IAppUserManager appUserManager)
        {
            _appUserManager = appUserManager;
        }
        public async Task<AppUsersResponseDto> GetAllUsersAsync()
        {
            try
            {
                var users = await _appUserManager.GetAllUsersAsync();

                return new AppUsersResponseDto
                {
                    Success = true,
                    Data = users.Select(user => new GetAccountDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email
                        
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                return new AppUsersResponseDto { Success = false, Errors = new[] { e.Message } };
            }
        }

        public async Task<AppUserResponseDto> GetUserByEmail(string email)
        {
            try
            {
                var user = await _appUserManager.GetUserByEmailAsync(email);

                return new AppUserResponseDto { 
                    Success = true, 
                    Data = new GetAccountDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email
                    }
                };
            }
            catch (Exception e)
            {
                return new AppUserResponseDto { Success = false, Errors = new[] { e.Message } };
            }
        }
    }
}