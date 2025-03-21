using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract;

public interface IColorRepository : IBaseRepository<Color>
{
    Task<Color> GetByNameAsync(string name);
    Task<Color> GetByIdAsync(int id);
}
