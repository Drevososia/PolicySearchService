using System.Data;
using Microsoft.Data.SqlClient;

namespace PolicySearchService.Data;

public abstract class DapperDbContext
{
    private readonly string _connectionString;

    protected DapperDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
