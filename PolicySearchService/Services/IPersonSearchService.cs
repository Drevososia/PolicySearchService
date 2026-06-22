using PolicySearchService.Models;

namespace PolicySearchService.Services;

public interface IPersonSearchService
{
    Task<Person?> SearchByEnpAsync(string enp);
}
