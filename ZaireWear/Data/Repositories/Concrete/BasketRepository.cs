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

public class BasketRepository : BaseRepository<Basket>, IBasketRepository
{
    private readonly AppDbContext _context;

    public BasketRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Basket> GetBasketByUserIdWithDetails(string userId)
    {
        return await _context.Baskets
            .Include(b => b.BasketProducts)
                .ThenInclude(bp => bp.Product)
                    .ThenInclude(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task<Basket> GetBasketByUserId(string userId)
    {
        return await _context.Baskets
            .Include(b => b.BasketProducts)
                .ThenInclude(bp => bp.Product)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task CreateBasketForUserAsync(string userId)
    {
        var basket = new Basket
        {
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        await _context.Baskets.AddAsync(basket);
    }
}