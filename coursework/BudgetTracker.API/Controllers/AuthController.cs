using Microsoft.AspNetCore.Mvc;
using BudgetTracker.API.DTOs;
using BudgetTracker.API.Services;

namespace BudgetTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var token = await _authService.RegisterAsync(registerDto);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new { token });
    }
}

