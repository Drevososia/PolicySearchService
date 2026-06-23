using Dapper;
using PolicySearchService.Data;
using PolicySearchService.Models.Person;

namespace PolicySearchService.Repositories.PersonRepository;

public class PersonRepository : IPersonRepository
{
    private readonly DapperDbContext _context;

    public PersonRepository(DapperDbContext context)
    {
        _context = context;
    }

    public async Task<Person?> GetByEnpAsync(string enp)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Person>(
            "SELECT FAM, IM, OT, W, DR, DS, Q as SMO, SPOL, NPOL, ENP, OPDOC, ID FROM PEOPLE WHERE ENP = @Enp",
            new { Enp = enp });
    }
}
