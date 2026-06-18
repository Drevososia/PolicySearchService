using Microsoft.AspNetCore.Mvc;
using PolicySearchService.Models;
using PolicySearchService.Services;

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
        _logger.LogInformation("Login attempt for user: {Login}, org: {OrgCode}", request.Login, request.OrgCode);

        var result = _authService.Authenticate(request);
        if (result is null)
        {
            _logger.LogWarning("Failed login for user: {Login}", request.Login);
            return Unauthorized(new { message = "Invalid credentials" });
        }

        _logger.LogInformation("Successful login for user: {Login}", request.Login);
        return Ok(result);
    }
}
