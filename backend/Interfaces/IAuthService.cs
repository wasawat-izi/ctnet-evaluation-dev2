using System.Threading.Tasks;
using backend.Dtos.Accounts;
using backend.Dtos.Auth;

namespace backend.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterUserAsync(RegisterAccountDto dto);
        Task<AuthResultDto> LoginUserAsync(LoginAccountDto dto);
    }
}