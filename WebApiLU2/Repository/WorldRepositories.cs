namespace WebApiLU2.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using System.Data;
    using WebApiLU2.Data;
    using WebApiLU2.Models;

    public class WorldRepository
    {
        private readonly DapperDbContext _db;

        public WorldRepository(DapperDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Environment2D>> GetUserWorldsAsync(string userId)
        {
            using var connection = _db.CreateConnection();
            return await connection.QueryAsync<Environment2D>(
                "SELECT * FROM Environments WHERE UserId = @UserId", new { UserId = userId });
        }

        public async Task<int> CreateWorldAsync(Environment2D world)
        {
            using var connection = _db.CreateConnection();
            return await connection.ExecuteAsync(
                "INSERT INTO Environments (Id, Name, MaxHeight, MaxLength, UserId) VALUES (@Id, @Name, @MaxHeight, @MaxLength, @UserId)", world);
        }
    }    
}

