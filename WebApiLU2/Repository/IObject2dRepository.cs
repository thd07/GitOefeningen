using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public interface IObject2dRepository
    {
        Task<Object2D> InsertAsync(Object2D objectModel);
        Task<Object2D> ReadAsyncId(Guid id);
        Task<IEnumerable<Object2D>> ReadAsync();
        Task UpdateAsync(Object2D objectModel);
        Task DeleteAsync(Guid id);
    }
}