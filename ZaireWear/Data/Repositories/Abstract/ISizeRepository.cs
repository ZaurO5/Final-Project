using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract;

public interface ISizeRepository : IBaseRepository<Size>
{
    Task<Size> GetByNameAsync(string name);
    Task<Size> GetByIdAsync(int id);
}
