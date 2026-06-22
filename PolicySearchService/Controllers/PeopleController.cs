using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicySearchService.Models;
using PolicySearchService.Repositories;

namespace PolicySearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(IPersonRepository repository, ILogger<PeopleController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("policy/{enp}")]
    public async Task<ActionResult<PolicySearchResponse>> GetByPolicy(string enp)
    {
        _logger.LogInformation("Поиск ЗЛ по енп: {Enp}", enp);

        var person = await _repository.GetByEnpAsync(enp);
        if (person is null)
        {
            _logger.LogWarning("ЗЛ с ЕНП {Enp} не найден", enp);
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
