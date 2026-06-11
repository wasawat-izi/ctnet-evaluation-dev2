using System.Threading.Tasks;
using backend.Dtos.Accounts;

namespace backend.Interfaces
{
    public interface IAuthService
    {
        Task<NewAppUserResponseDto> RegisterUserAsync(RegisterAccountDto dto);
        Task<NewAppUserResponseDto> LoginUserAsync(LoginAccountDto dto);
    }
}