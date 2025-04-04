using Microsoft.AspNetCore.Mvc;
using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public interface IEnvironment2dRepository
    {
        Task<List<Environment2D>> ReadWorldsAsync(Guid userId);
        Task DeleteEnvironmentAsync(Guid UserId, Guid WorldId);
        Task<Environment2D> CreateWorldAsync([FromBody] Environment2D model ,Guid userId);
    }
}
