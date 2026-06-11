using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Dtos.Accounts;

namespace backend.Interfaces
{
    public interface IAppUserService
    {
        Task<AppUsersResponseDto> GetAllUsersAsync();
        Task<AppUserResponseDto> GetUserByEmail(string email);
    }
}