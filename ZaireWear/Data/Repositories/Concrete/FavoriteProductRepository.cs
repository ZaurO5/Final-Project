using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete;

public class FavoriteProductRepository : IFavoriteProductRepository
{
    private readonly AppDbContext _context;

    public FavoriteProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FavoriteProduct> GetByUserAndProductAsync(string userId, int productId)
    {
        return await _context.FavoriteProducts
            .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.ProductId == productId);
    }

    public async Task AddAsync(FavoriteProduct favoriteProduct)
    {
        await _context.FavoriteProducts.AddAsync(favoriteProduct);
    }

    public void Remove(FavoriteProduct favoriteProduct)
    {
        _context.FavoriteProducts.Remove(favoriteProduct);
    }

    public async Task<List<FavoriteProduct>> GetByUserIdAsync(string userId)
    {
        return await _context.FavoriteProducts
            .Include(fp => fp.Product)
            .Where(fp => fp.UserId == userId)
            .ToListAsync();
    }
}
