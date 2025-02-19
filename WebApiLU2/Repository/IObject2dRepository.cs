using WebApiLU2.Models;

namespace WebApiLU2.Repository
{
    public interface IObject2dRepository
    {
        Task<Object2dModel> InsertAsync(Object2dModel objectModel);
        Task<Object2dModel> ReadAsyncId(Guid id);
        Task<IEnumerable<Object2dModel>> ReadAsync();
        Task UpdateAsync(Object2dModel objectModel);
        Task DeleteAsync(Guid id);
    }
}