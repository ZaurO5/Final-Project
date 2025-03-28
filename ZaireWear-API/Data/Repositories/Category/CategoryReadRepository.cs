using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Category
{
    public class CategoryReadRepository : BaseReadRepository<Core.Entities.Category>, ICategoryReadRepository
    {
        private readonly AppDbContext _context;

        public CategoryReadRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Core.Entities.Category> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Core.Entities.Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
