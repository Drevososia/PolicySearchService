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
        _logger.LogInformation("Searching person by ENP: {Enp}", enp);

        var person = await _repository.GetByEnpAsync(enp);
        if (person is null)
        {
            _logger.LogWarning("Person with ENP {Enp} not found", enp);
            return NotFound(new { message = "Person not found" });
        }

        var response = new PolicySearchResponse
        {
            FAM = person.FAM,
            IM = person.IM,
            OT = person.OT,
            W = person.W,
            DR = person.DR,
            DS = person.DS,
            Q = person.Q,
            SPOL = person.SPOL,
            NPOL = person.NPOL,
            ENP = person.ENP,
            OPDOC = person.OPDOC
        };

        _logger.LogInformation("Found person: {Fam} {Im} {Ot}, ENP: {Enp}", person.FAM, person.IM, person.OT, enp);
        return Ok(response);
    }   
}
