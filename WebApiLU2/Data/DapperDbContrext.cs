namespace WebApiLU2.Data
{
    using Dapper;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Data.SqlClient;

    public class DapperDbContext
    {
        private readonly IConfiguration _config;

        public DapperDbContext(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection (_config.GetConnectionString("SqlConnectionString"));
        }
    }
}
