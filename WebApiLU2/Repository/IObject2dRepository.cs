using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public interface IObject2dRepository
    {
        Task<Object2D> InsertAsync(Object2D object2dModel, Guid UserId);
        Task<Object2D> ReadAsyncId(Guid WorldId, Guid UserId);
        Task UpdateAsync(Object2D objectModel);
        Task DeleteAllAsync(Guid WorldId, Guid UserId);
    }
}