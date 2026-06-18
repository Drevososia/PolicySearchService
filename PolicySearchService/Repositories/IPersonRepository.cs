using PolicySearchService.Models;

namespace PolicySearchService.Repositories;

public interface IPersonRepository
{
    Task<Person?> GetByEnpAsync(string enp);
}
