using Data.Contexts;
using Data.Repositories.Base;

namespace Data.Repositories.Category
{
    public class CategoryWriteRepository : BaseWriteRepository<Core.Entities.Category>, ICategoryWriteRepository
    {
        public CategoryWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
