using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAuthManager
    {
        Task<(AppUser, string)> RegisterNewUserAsync(AppUser appUser, string password);
        Task<(AppUser, string)> SignInUserByEmailAsync(string email, string password);
        Task<(AppUser, string)> SignInUserByUsernameAsync(string username, string password);
    }
}