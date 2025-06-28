using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public interface IObject2dRepository
    {
        Task<Object2D> InsertAsync(Object2D object2dModel);
        Task<IEnumerable<Object2D>> ReadAsyncId(Guid WorldId);
        Task UpdateAsync(Object2D objectModel);
        Task DeleteAllAsync(Guid WorldId);
        Task DeleteObjectAsync(Guid WorldId, Guid ObjectId);
    }
}