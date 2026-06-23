using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicySearchService.Services.AttachedPeopleImportService;

namespace PolicySearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttachedPeopleController : ControllerBase
{
    private readonly IAttachedPeopleImportService _importService;
    private readonly ILogger<AttachedPeopleController> _logger;

    public AttachedPeopleController(IAttachedPeopleImportService importService, ILogger<AttachedPeopleController> logger)
    {
        _importService = importService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("import")]
    [RequestSizeLimit(500 * 1024 * 1024)]
    public async Task<ActionResult> Import(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран или пуст" });

        _logger.LogInformation("Загрузка файла {FileName} размером {Length} байт", file.FileName, file.Length);

        try
        {
            var count = await ImportFileAsync(file);
            return Ok(new { message = $"Импортировано {count} записей", count });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка импорта файла {FileName}", file.FileName);
            return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
        }
    }

    private async Task<int> ImportFileAsync(IFormFile file)
    {
        if (file.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
        {
            await using var stream = file.OpenReadStream();
            return await _importService.ImportFromArchiveAsync(stream);
        }

        if (file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            await using var stream = file.OpenReadStream();
            return await _importService.ImportFromCsvAsync(stream);
        }

        throw new InvalidOperationException("Поддерживаются только CSV и ZIP файлы");
    }
}
