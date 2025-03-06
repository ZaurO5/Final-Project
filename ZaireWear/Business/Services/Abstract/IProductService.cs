using Business.ViewModels.Product;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IProductService
    {
        Task<ProductIndexVM> GetAllAsync();
        Task<Product> GetByIdWithDetailsAsync(int id);
        Task<ProductCreateVM> CreateAsync();
        Task<bool> CreateAsync(ProductCreateVM model);
        Task<ProductUpdateVM> UpdateAsync(int id);
        Task<bool> UpdateAsync(int id, ProductUpdateVM model);
        Task<bool> DeleteAsync(int id);
    }
}
