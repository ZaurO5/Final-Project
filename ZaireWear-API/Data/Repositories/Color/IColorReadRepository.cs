using Data.Repositories.Base;

namespace Data.Repositories.Color
{
    public interface IColorReadRepository : IBaseReadRepository<Core.Entities.Color>
    {
        Task<Core.Entities.Color> GetByNameAsync(string name);
        Task<Core.Entities.Color> GetByIdAsync(int id);
    }

}
