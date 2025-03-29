using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Color
{
    public class ColorReadRepository : BaseReadRepository<Core.Entities.Color>, IColorReadRepository
    {
        private readonly AppDbContext _context;

        public ColorReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Core.Entities.Color> GetByNameAsync(string name)
        {
            return await _context.Colors.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Core.Entities.Color> GetByIdAsync(int id)
        {
            return await _context.Colors.FindAsync(id);
        }
    }

}
