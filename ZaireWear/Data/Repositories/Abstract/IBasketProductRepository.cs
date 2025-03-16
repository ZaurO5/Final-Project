using Core.Entities;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract;

public interface IBasketProductRepository 
{
    //Task<BasketProduct> GetByProductIdColorSizeAndUserIdAsync(int productId, string color, string size, string userId);
    Task<List<BasketProduct>> GetBasketProductsWithProducts(int userBasketId);
    Task<BasketProduct> GetByIdAsync(int id);
    Task<BasketProduct> GetByVariantAsync(int basketId, int productId, int ColorId, int SizeId);

    Task<List<BasketProduct>> GetAllAsync();
    Task CreateAsync(BasketProduct data);
    void Update(BasketProduct data);
    void Delete(BasketProduct data);
}