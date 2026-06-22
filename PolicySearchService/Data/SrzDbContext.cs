namespace PolicySearchService.Data;

public class SrzDbContext : DapperDbContext
{
    public SrzDbContext(string connectionString) : base(connectionString) { }
}
