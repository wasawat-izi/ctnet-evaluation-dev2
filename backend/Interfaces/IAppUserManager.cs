using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAppUserManager
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<List<AppUser>> GetAllUsersAsync();
    }
}