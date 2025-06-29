using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using WebApiLU2.Models;
using Microsoft.Data.SqlClient;


namespace WebApiLU2.Repository

{
    public class Object2dRepository : IObject2dRepository
    {
        private readonly string sqlConnectionString;
        public Object2dRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }


        public async Task<Object2D> InsertAsync(Object2D object2dModel)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);

            // Check if the environment exists
            var environmentExists = await sqlConnection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM [2dEnvironment] WHERE Id = @Id", new { Id = object2dModel.IdEnvironment });

            if (environmentExists == 0)
            {
                throw new Exception($"Environment with Id {object2dModel.IdEnvironment} does not exist.");
            }

            object2dModel.IdObject = Guid.NewGuid();

            await sqlConnection.ExecuteAsync(
                "INSERT INTO [Object2D] (IdEnvironment, PrefabId, PosX, PosY, ScaleX, ScaleY, RotationZ, SortingLayer, IdObject) VALUES (@IdEnvironment, @PrefabId, @PosX, @PosY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @IdObject)",
                object2dModel);

            return object2dModel;
        }


        public async Task<IEnumerable<Object2D>> ReadAsyncId(Guid WorldId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var allObjects = await sqlConnection.QueryAsync<Object2D>(
                    "SELECT * FROM [Object2D] WHERE IdEnvironment = @IdEnvironment",
                    new {IdEnvironment = WorldId });

                return allObjects;
            }
        }


        public async Task UpdateAsync(Object2D object2d)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
    "UPDATE [Object2D] SET " +
    "PrefabId = @PrefabId, " +
    "PosX = @PosX, " +
    "PosY = @PosY, " +
    "ScaleX = @ScaleX, " +
    "ScaleY = @ScaleY, " +
    "RotationZ = @RotationZ, " +
    "SortingLayer = @SortingLayer " +  // <-- No comma here
    "WHERE IdEnvironment = @IdEnvironment AND IdObject = @IdObject",
    new
    {
        object2d.PrefabId,
        object2d.PosX,
        object2d.PosY,
        object2d.ScaleX,
        object2d.ScaleY,
        object2d.RotationZ,
        object2d.SortingLayer,
        object2d.IdEnvironment,  // You need to pass this parameter too
        object2d.IdObject       // And this one
    });


            }
        } 

        public async Task DeleteAllAsync(Guid WorldId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE IdEnvironment = @IdEnvironment", new {IdEnvironment = WorldId });
            }
        }
        public async Task DeleteObjectAsync(Guid WorldId, Guid ObjectId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Object2D] WHERE IdEnvironment = @IdEnvironment AND IdObject = @IdObject",
                    new { IdEnvironment = WorldId, IdObject = ObjectId }
                );
            }
        }



    }
}
