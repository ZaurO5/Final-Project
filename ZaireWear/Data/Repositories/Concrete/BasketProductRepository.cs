using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete;

public class BasketProductRepository : IBasketProductRepository
{
    private readonly AppDbContext _context;

    public BasketProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BasketProduct>> GetBasketProductsWithProducts(int userBasketId)
    {
        return await _context.BasketProducts
            .Include(bp => bp.Product)
            .Where(bp => bp.BasketId == userBasketId)
            .ToListAsync();
    }

    public async Task<BasketProduct> GetByIdAsync(int basketId, int productId, int colorId, int sizeId)
    {
        return await _context.BasketProducts
            .FirstOrDefaultAsync(bp =>
                bp.BasketId == basketId &&
                bp.ProductId == productId &&
                bp.ColorId == colorId &&
                bp.SizeId == sizeId);
    }

    public async Task<BasketProduct> GetByVariantAsync(
        int basketId,
        int productId,
        int colorId,
        int sizeId)
    {
        return await _context.BasketProducts
            .FirstOrDefaultAsync(bp =>
                bp.BasketId == basketId &&
                bp.ProductId == productId &&
                bp.ColorId == colorId &&
                bp.SizeId == sizeId);
    }

    public async Task<List<BasketProduct>> GetAllAsync()
    {
        return await _context.BasketProducts.ToListAsync();
    }

    public async Task CreateAsync(BasketProduct data)
    {
        await _context.BasketProducts.AddAsync(data);
    }

    public void Update(BasketProduct data)
    {
        _context.BasketProducts.Update(data);
    }

    public void Delete(BasketProduct data)
    {
        _context.BasketProducts.Remove(data);
    }
}