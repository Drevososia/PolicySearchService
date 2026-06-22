using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicySearchService.Models;
using PolicySearchService.Services;

namespace PolicySearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonSearchService _searchService;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(IPersonSearchService searchService, ILogger<PeopleController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("policy/{enp}")]
    public async Task<ActionResult<PolicySearchResponse>> GetByPolicy(string enp)
    {
        _logger.LogInformation("Поиск ЗЛ по енп: {Enp}", enp);

        var person = await _searchService.SearchByEnpAsync(enp);
        if (person is null)
        {
            _logger.LogWarning("ЗЛ с ЕНП {Enp} не найден ни в одном источнике", enp);
            return NotFound(new { message = "ЗЛ не найден" });
        }

        var response = new PolicySearchResponse
        {
            FAM = person.FAM,
            IM = person.IM,
            OT = person.OT,
            W = person.W,
            DR = person.DR,
            DS = person.DS,
            SMO = person.SMO,
            SPOL = person.SPOL,
            NPOL = person.NPOL,
            ENP = person.ENP,
            OPDOC = person.OPDOC
        };

        _logger.LogInformation("Найден ЗЛ: {Fam} {Im} {Ot}, ЕНП: {Enp}", person.FAM, person.IM, person.OT, enp);
        return Ok(response);
    }   
}
