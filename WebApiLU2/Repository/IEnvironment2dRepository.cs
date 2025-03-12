using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public interface IEnvironment2dRepository
    {
        Task<List<Environment2D>> GetOwnerWorlds(string userId);
    }
}
