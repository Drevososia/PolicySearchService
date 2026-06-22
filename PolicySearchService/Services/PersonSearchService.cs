using PolicySearchService.Models;
using PolicySearchService.Repositories;

namespace PolicySearchService.Services;

public class PersonSearchService : IPersonSearchService
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<PersonSearchService> _logger;

    public PersonSearchService(IPersonRepository repository, ILogger<PersonSearchService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Person?> SearchByEnpAsync(string enp)
    {
        var person = await _repository.GetByEnpAsync(enp);
        if (person is null)
            _logger.LogWarning("ЗЛ с ЕНП {Enp} не найден", enp);
        return person;
    }
}
