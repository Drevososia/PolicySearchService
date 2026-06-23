using Microsoft.AspNetCore.Mvc;
using PolicySearchService.Models.Authorization;
using PolicySearchService.Services.AuthService;

namespace PolicySearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(AuthRequest request)
    {
        _logger.LogInformation("Попытка логина для пользователя: {Login}, Код организации: {OrgCode}", request.Login, request.OrgCode);

        var result = _authService.Authenticate(request);
        if (result is null)
        {
            _logger.LogWarning("Ошибка входа для пользователя: {Login}", request.Login);
            return Unauthorized(new { message = "Invalid credentials" });
        }

        _logger.LogInformation("Успешная попытка логина для пользователя: {Login}", request.Login);
        return Ok(result);
    }
}
