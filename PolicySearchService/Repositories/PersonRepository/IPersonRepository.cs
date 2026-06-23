using PolicySearchService.Models.Person;

namespace PolicySearchService.Repositories.PersonRepository;

public interface IPersonRepository
{
    Task<Person?> GetByEnpAsync(string enp);
}
