using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete;

public class SizeRepository : BaseRepository<Size>, ISizeRepository
{
    private readonly AppDbContext _context;

    public SizeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Size> GetByIdAsync(int id)
    {
        return await _context.Sizes.FindAsync(id);
    }

    public async Task<Size> GetByNameAsync(string name)
    {
        return await _context.Sizes.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
    }
}
