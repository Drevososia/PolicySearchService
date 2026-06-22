namespace PolicySearchService.Data;

public class AttachedPersonDbContext : DapperDbContext
{
    public AttachedPersonDbContext(string connectionString) : base(connectionString) { }
}
