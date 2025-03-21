using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category> GetByNameAsync(string name);
    Task<Category> GetByIdAsync(int id);
}
