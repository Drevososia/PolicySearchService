using PolicySearchService.Models;
using PolicySearchService.Repositories;

namespace PolicySearchService.Services;

public class PersonSearchService : IPersonSearchService
{
    private readonly IEnumerable<IPersonRepository> _repositories;
    private readonly ILogger<PersonSearchService> _logger;

    public PersonSearchService(IEnumerable<IPersonRepository> repositories, ILogger<PersonSearchService> logger)
    {
        _repositories = repositories;
        _logger = logger;
    }

    public async Task<Person?> SearchByEnpAsync(string enp)
    {
        foreach (var repo in _repositories)
        {
            var person = await repo.GetByEnpAsync(enp);
            if (person is not null)
                return person;

            _logger.LogInformation("ЗЛ с ЕНП {Enp} не найден в текущем источнике, пробуем следующий", enp);
        }

        _logger.LogWarning("ЗЛ с ЕНП {Enp} не найден ни в одном источнике", enp);
        return null;
    }
}
