using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using WebApiLU2.Data;
using WebApiLU2.Models;
using WebApiLU2.DTOs;
using System.Security.Claims;

[Route("api/environment")]
[ApiController]
[Authorize]
public class EnvironmentController : ControllerBase
{
    private readonly DapperDbContext _dbContext;

    public EnvironmentController(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEnvironments()
    {
        using var connection = _dbContext.CreateConnection();
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var environments = await connection.QueryAsync<Environment2D>(
            "SELECT * FROM 2dEnvironment WHERE UserId = @UserId", new { UserId = userId });

        return Ok(environments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnvironment([FromBody] Environment2D model)
    {
        using var connection = _dbContext.CreateConnection();
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var newEnvironment = new Environment2D
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            MaxHeight = model.MaxHeight,
            MaxLength = model.MaxLength,
            UserId = userId
        };

        await connection.ExecuteAsync(
            "INSERT INTO 2dEnvironment (Id, Name, MaxHeight, MaxLength, UserId) VALUES (@Id, @Name, @MaxHeight, @MaxLength, @UserId)", newEnvironment);

        return Ok(new { success = "2D-wereld succesvol aangemaakt!", id = newEnvironment.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnvironment(Guid id)
    {
        using var connection = _dbContext.CreateConnection();

        int rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM 2dEnvironment WHERE Id = @Id", new { Id = id });

        if (rowsAffected == 0)
            return NotFound(new { error = "2D-wereld niet gevonden." });

        return Ok(new { success = "2D-wereld succesvol verwijderd!" });
    }
}
