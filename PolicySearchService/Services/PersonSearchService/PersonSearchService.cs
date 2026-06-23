using PolicySearchService.Models.Person;
using PolicySearchService.Repositories.PersonRepository;

namespace PolicySearchService.Services.PersonSearchService;

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
        return await _repository.GetByEnpAsync(enp);
    }
}
