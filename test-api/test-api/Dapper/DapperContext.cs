using System.Data;
using System.Data.SqlClient;

namespace test_api.Dapper
{
    public class DapperContext
    {
        private IDbConnection _connection;
        private string _connectionString;

        public DapperContext(IDbConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _connectionString = configuration.GetConnectionString("AdventureWorks");
        }

        public IDbConnection GetConnection() => new SqlConnection(_connectionString);
    }
}
