using Microsoft.Data.SqlClient;
using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public class Environment2dRepository : IEnvironment2dRepository
    {
        private readonly string sqlConnectionString;
        public Environment2dRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }
        public async Task<List<Environment2D>> GetOwnerWorlds(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var OwnerWorlds = (Query: "SELECT * FROM Environment2d WHERE OwnerId = @OwnerId", new { OwnerId = userId });
                return (OwnerWorlds);
            }

        }

    }
}
