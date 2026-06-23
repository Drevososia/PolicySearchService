using Dapper;
using PolicySearchService.Data;
using PolicySearchService.Models.Person;

namespace PolicySearchService.Repositories.PersonRepository;

public class AttachedPeopleRepository : IPersonRepository
{
    private readonly DapperDbContext _context;

    public AttachedPeopleRepository(DapperDbContext context)
    {
        _context = context;
    }

    public async Task<Person?> GetByEnpAsync(string enp)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Person>(
            @"SELECT FAM, IM, OT, GENDER as W, DR, DATE_DEATH as DS, SMO, ENP
              FROM AttachedPeople
              WHERE ENP = @Enp",
            new { Enp = enp });
    }
}
