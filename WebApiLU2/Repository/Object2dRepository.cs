﻿using Dapper;
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

 
        public async Task<Object2dModel> InsertAsync(Object2dModel object2dModel)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var Object2dWithInfo = await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (IdEnvironment, PrefabId, PosX, PosY, ScaleX, ScaleY, RotationZ, SortingLayer, IdObject ) VALUES (@IdEnvironment, @PrefabId, @PosX, @PosY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @IdObject)", object2dModel);
                return object2dModel;
            }
        }

        public async Task<Object2dModel> ReadAsyncId(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2dModel>("SELECT * FROM [Object2D] WHERE IdEnvironment = @IdEnvironment", new { id });
            }
        }

        public async Task<IEnumerable<Object2dModel>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2dModel>("SELECT * FROM [Object2D]");
            }
        }

        public async Task UpdateAsync(Object2dModel object2d)
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

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }



    }
}
