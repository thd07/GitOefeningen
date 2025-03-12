using Dapper;
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
                // Open de verbinding naar de database
                await sqlConnection.OpenAsync();

                // Voer de query uit en haal de resultaten op
                var OwnerWorlds = await sqlConnection.QueryAsync<Environment2D>(
                    "SELECT * FROM Environment2d WHERE UserId = @UserId",
                    new { UserId = userId });

                // Zet de resultaten om naar een lijst en retourneer
                return OwnerWorlds.AsList();
            }
        }

    }
}
