using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonthlyExpenseTracker.Data;
using MonthlyExpenseTracker.DTO;
using MonthlyExpenseTracker.EntityModels;
using MonthlyExpenseTracker.Helper.Jwt;
using MonthlyExpenseTracker.Helper.ResponseVM;
using MonthlyExpenseTracker.Services.Interface;

namespace MonthlyExpenseTracker.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<tblUsers> _passwordHasher;
    private readonly JwtTokenHelper _jwtTokenHelper;

    public AuthService(AppDbContext context, JwtTokenHelper jwtTokenHelper, IPasswordHasher<tblUsers> passwordHasher)
    {
        _context = context;
        _jwtTokenHelper = jwtTokenHelper;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseVm<AuthResponseDTO>> RegisterAsync(RegisterRequestDTO model)
    {
        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            return new ResponseVm<AuthResponseDTO>("Email already registered.");

        
        var user = new tblUsers
        {
            Email = model.Email,
            Name = model.FullName,
            City = model.City,
            Country = model.Country,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            PasswordHash = "", // to be hashed
            RoleId = (await _context.Role.FirstAsync(r => r.Name == "User")).Id
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        string token =_jwtTokenHelper.GenerateToken(user);

        return new ResponseVm<AuthResponseDTO>(new AuthResponseDTO
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.Name,
            FullName = user.Name,
            City = user.City,
            Country = user.Country
        });
    }

    public async Task<ResponseVm<AuthResponseDTO>> LoginAsync(LoginRequestDTO model)
    {
        var user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
            return new ResponseVm<AuthResponseDTO>("Invalid credentials.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        if (result != PasswordVerificationResult.Success)
            return new ResponseVm<AuthResponseDTO>("Invalid credentials.");

        string token =_jwtTokenHelper.GenerateToken(user);

        return new ResponseVm<AuthResponseDTO>(new AuthResponseDTO
        {
            Token = token,
            Email = user.Email,
            Role = user.Role.Name,
            FullName = user.Name,
            City = user.City,
            Country= user.Country
        });
    }

}
