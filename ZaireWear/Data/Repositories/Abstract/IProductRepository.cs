using Core.Entities;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetAllWithDetailsAsync();
        Task<Product> GetByIdWithDetailsAsync(int id);
        Task AddAsync(Product product);
    }
}
