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

        public async Task<Object2D> ReadAsyncId(Guid WorldId, Guid UserId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
               var AllObjects = await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE UserId = @UserId AND IdEnvironment = @IdEnvironment", new { UserId, IdEnvironment = WorldId });

                return AllObjects;
            }
        }


        public async Task UpdateAsync(Object2D object2d)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Object2D] SET " +
                                                 "IdEnvironment = @IdEnvironment" +
                                                 "PrefabId = @PrefabId"+
                                                 "PosX = @PosX"+
                                                 "PosY = @PosY" +
                                                 "ScaleX = @ScaleX" +
                                                 "ScaleY = @ScaleY" +
                                                 "RotationZ = @RotationZ" +
                                                 "SortingLayer = @SortingLayer" +
                                                 "IdObject = @IdObject"
                                                 , object2d);
                //IdEnvironment, PrefabId, PosX, PosY, ScaleX, ScaleY, RotationZ, SortingLayer, IdObject
            }
        } 

        public async Task DeleteAllAsync(Guid WorldId, Guid userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE * FROM [Object2D] WHERE UserId = @UserId AND IdEnvironment = @IdEnvironment", new { UserId = userId, IdEnvironment = WorldId });
            }
        }



    }
}
