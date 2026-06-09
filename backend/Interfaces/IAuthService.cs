using System.Threading.Tasks;
using backend.Dtos.Accounts;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterUserAsync(RegisterAccountDto dto);
        Task<AuthResult> LoginUserAsync(LoginAccountDto dto);
    }
}