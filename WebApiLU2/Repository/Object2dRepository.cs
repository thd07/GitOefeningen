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

 
        public async Task<Object2D> InsertAsync(Object2D object2dModel, Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                object2dModel.UserId = UserId;
                object2dModel.IdObject = Guid.NewGuid();
                await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (IdEnvironment, PrefabId, PosX, PosY, ScaleX, ScaleY, RotationZ, SortingLayer, IdObject, UserId ) VALUES (@IdEnvironment, @PrefabId, @PosX, @PosY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @IdObject, @UserId)", object2dModel );
                return object2dModel;
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAsyncId(Guid WorldId, Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var allObjects = await sqlConnection.QueryAsync<Object2D>(
                    "SELECT * FROM [Object2D] WHERE UserId = @UserId AND IdEnvironment = @IdEnvironment",
                    new { UserId, IdEnvironment = WorldId });

                return allObjects;
            }
        }


        public async Task UpdateAsync(Object2D object2d, Guid UserId)
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
                    "SortingLayer = @SortingLayer, " +
                    "IdObject = @IdObject " +
                    "WHERE UserId = @UserId AND IdEnvironment = @IdEnvironment AND IdObject = @IdObject",
                    new
                    {
                        object2d.PrefabId,
                        object2d.PosX,
                        object2d.PosY,
                        object2d.ScaleX,
                        object2d.ScaleY,
                        object2d.RotationZ,
                        object2d.SortingLayer,
                        object2d.IdObject,
                        UserId,
                        object2d.IdEnvironment
                    });

                //IdEnvironment, PrefabId, PosX, PosY, ScaleX, ScaleY, RotationZ, SortingLayer, IdObject
            }
        } 

        public async Task DeleteAllAsync(Guid WorldId, Guid userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE UserId = @UserId AND IdEnvironment = @IdEnvironment", new { UserId = userId, IdEnvironment = WorldId });
            }
        }



    }
}
