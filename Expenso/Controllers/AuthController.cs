using Microsoft.AspNetCore.Mvc;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDTO register)
    {
        var result = await _authService.RegisterAsync(register);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO login)
    {
        var result = await _authService.LoginAsync(login);
        return Ok(result);
    }
}
