using System.Threading.Tasks;
using backend.Dtos.Accounts;

namespace backend.Interfaces
{
    public interface IAuthService
    {
        Task<NewAppUserResponseDto> RegisterUserAsync(RegisterAccountDto dto);
        Task<NewAppUserResponseDto> LoginUserByEmailAsync(LoginAccountByEmailDto dto);
        Task<NewAppUserResponseDto> LoginUserByUsernameAsync(LoginAccountByUsernameDto dto);
    }
}