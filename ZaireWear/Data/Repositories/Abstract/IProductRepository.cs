using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetAllWithDetailsAsync();
    Task<Product> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Product product);
    Task<Product> GetByIdAsync(int id);
}
