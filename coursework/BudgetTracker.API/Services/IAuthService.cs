using BudgetTracker.API.DTOs;

namespace BudgetTracker.API.Services;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto registerDto);
    Task<string?> LoginAsync(LoginDto loginDto);
}

