using ValeraProject.DTOs;
using ValeraProject.Models;

namespace ValeraProject.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(User user);
    }
}

