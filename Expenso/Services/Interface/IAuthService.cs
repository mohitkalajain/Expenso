using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.Helper.ResponseVM;

namespace MonthlyExpenseTracker.Services.Interface;

public interface IAuthService
{
    Task<ResponseVm<AuthResponseDTO>> RegisterAsync(RegisterRequestDTO model);
    Task<ResponseVm<AuthResponseDTO>> LoginAsync(LoginRequestDTO model);
}
