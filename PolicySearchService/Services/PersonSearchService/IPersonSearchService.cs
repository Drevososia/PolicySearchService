using PolicySearchService.Models.Person;

namespace PolicySearchService.Services.PersonSearchService;

public interface IPersonSearchService
{
    Task<Person?> SearchByEnpAsync(string enp);
}
