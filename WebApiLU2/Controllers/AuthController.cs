using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiLU2.Models;
using WebApiLU2.Services;

[Route("account")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var (success, message) = await _authService.RegisterAsync(user.Username, user.Password);

        if (!success)
            return BadRequest(new { error = message });

        return Ok(new { success = message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var (success, message) = await _authService.LoginAsync(user.Username, user.Password);

        if (!success)
            return Unauthorized(new { error = message });

        return Ok(new { success = "Login succesvol!" });
    }
}
