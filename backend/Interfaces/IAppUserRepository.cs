using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(AppUser user, string password);
        Task<IdentityResult> AddUserRoleAsync(AppUser user, string role);
        Task<List<AppUser>?> GetAllUsersAsync();

        Task<SignInResult> CheckSignInPassword(AppUser user, string password);

    }
}