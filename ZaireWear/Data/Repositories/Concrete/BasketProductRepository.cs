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

public class BasketProductRepository : BaseRepository<BasketProduct>, IBasketProductRepository
{
    private readonly AppDbContext _context;

    public BasketProductRepository(AppDbContext context) : base(context)
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

    public async Task<BasketProduct> GetByProductIdColorSizeAndUserIdAsync(
    int productId,
    string color,
    string size,
    string userId)
    {
        return await _context.BasketProducts
            .Include(bp => bp.Basket)
            .Include(bp => bp.Product)
            .FirstOrDefaultAsync(bp =>
                bp.ProductId == productId &&
                bp.Color.Trim().ToLower() == color.Trim().ToLower() &&
                bp.Size.Trim().ToLower() == size.Trim().ToLower() &&
                bp.Basket.UserId == userId);
    }

    public async Task<BasketProduct> GetByIdAsync(int id)
    {
        return await _context.BasketProducts.FindAsync(id);
    }

    public async Task<BasketProduct> GetByVariantAsync(
    int basketId,
    int productId,
    string color,
    string size)
    {
        return await _context.BasketProducts
            .FirstOrDefaultAsync(bp =>
                bp.BasketId == basketId &&
                bp.ProductId == productId &&
                bp.Color == color &&
                bp.Size == size);
    }
}