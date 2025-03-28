using Data.Repositories.Base;

namespace Data.Repositories.Size
{
    public interface ISizeReadRepository : IBaseReadRepository<Core.Entities.Size>
    {
        Task<Core.Entities.Size> GetByNameAsync(string name);
        Task<Core.Entities.Size> GetByIdAsync(int id);
    }
}
