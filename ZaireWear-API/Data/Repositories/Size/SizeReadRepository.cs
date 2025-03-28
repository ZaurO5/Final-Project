using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Size
{
    public class SizeReadRepository : BaseReadRepository<Core.Entities.Size>, ISizeReadRepository
    {
        private readonly AppDbContext _context;

        public SizeReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Core.Entities.Size> GetByNameAsync(string name)
        {
            return await _context.Sizes.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Core.Entities.Size> GetByIdAsync(int id)
        {
            return await _context.Sizes.FindAsync(id);
        }
    }
}
