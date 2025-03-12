using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiLU2.Data;
using WebApiLU2.Models;

[Route("api/objects")]
[ApiController]
[Authorize]
public class ObjectController : ControllerBase
{
    private readonly DapperDbContext _dbContext;

    public ObjectController(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{environmentId}")]
    public async Task<IActionResult> GetObjects(Guid environmentId)
    {
        using var connection = _dbContext.CreateConnection();

        var objects = await connection.QueryAsync<Object2D>(
            "SELECT * FROM Object2D WHERE IdEnvironment = @EnvironmentId", new { EnvironmentId = environmentId });

        return Ok(objects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateObject([FromBody] Object2D model)
    {
        using var connection = _dbContext.CreateConnection();

        var newObject = new Object2D
        {
            IdObject = Guid.NewGuid(),
            IdEnvironment = model.IdEnvironment,
            PrefabId = model.PrefabId,
            PosX = model.PosX,
            PosY = model.PosY,
            ScaleX = model.ScaleX,
            RotationZ = model.RotationZ,
            SortingLayer = model.SortingLayer
        };

        await connection.ExecuteAsync(
            "INSERT INTO Object2D (IdObject, IdEnvironment, PrefabId, PosX, PosY, ScaleX, RotationZ, SortingLayer) VALUES (@IdObject, @IdEnvironment, @PrefabId, @PosX, @PosY, @ScaleX, @RotationZ, @SortingLayer)", newObject);

        return Ok(new { success = "Object succesvol toegevoegd!", id = newObject.IdObject });
    }
}
