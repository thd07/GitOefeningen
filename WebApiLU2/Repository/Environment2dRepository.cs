using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Environment2D>> ReadWorldsAsync(Guid userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
              

                // Voer de query uit en haal de resultaten op
                var OwnerWorlds = await sqlConnection.QueryAsync<Environment2D>(
                    "SELECT * FROM [2dEnvironment] WHERE UserId = @UserId",
                    new { UserId = userId });

                // Zet de resultaten om naar een lijst en retourneer
                return OwnerWorlds.ToList();
            }
        }


        public async Task DeleteEnvironmentAsync(Guid userId, Guid worldId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                int rowsAffected = await sqlConnection.ExecuteAsync(
                    "DELETE FROM [2dEnvironment] WHERE UserId = @UserId AND Id = @Id",
                    new { UserId = userId, Id = worldId });
            }
        }


        public async Task<Environment2D> CreateWorldAsync([FromBody] Environment2D model, Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var newWorld = new Environment2D
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    Name = model.Name,
                    MaxHeight = model.MaxHeight,
                    MaxLength = model.MaxLength
                };
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [2dEnvironment] (Id, UserId, Name, MaxHeight, MaxLength) VALUES (@Id, @UserId, @Name, @MaxHeight, @MaxLength)", newWorld);
                return newWorld;
            }
        }

    }

}