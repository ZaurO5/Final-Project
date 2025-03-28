using Data.Repositories.Base;

namespace Data.Repositories.Category
{
    public interface ICategoryReadRepository : IBaseReadRepository<Core.Entities.Category>
    {
        Task<Core.Entities.Category> GetByNameAsync(string name);
        Task<Core.Entities.Category> GetByIdAsync(int id);
    }
}
