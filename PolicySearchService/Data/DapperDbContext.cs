using Microsoft.Data.SqlClient;

namespace PolicySearchService.Data;

public class DapperDbContext
{
    private readonly string _connectionString;

    public DapperDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection() => new SqlConnection(_connectionString);
}
