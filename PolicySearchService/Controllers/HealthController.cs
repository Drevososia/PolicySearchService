using Dapper;
using Microsoft.AspNetCore.Mvc;
using PolicySearchService.Data;

namespace PolicySearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly SrzDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(SrzDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Check()
    {
        try
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("SELECT 1");

            _logger.LogInformation("Проверка подключения к БД: Успешно!");
            return Ok(new { status = "healthy", database = "connected" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Проверка подключения к БД: Подключение не установлено!");
            return StatusCode(500, new { status = "unhealthy", database = "disconnected", error = ex.Message });
        }
    }
}
