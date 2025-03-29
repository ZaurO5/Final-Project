using Data.Contexts;
using Data.Repositories.Base;

namespace Data.Repositories.Color
{
    public class ColorWriteRepository : BaseWriteRepository<Core.Entities.Color>, IColorWriteRepository
    {
        public ColorWriteRepository(AppDbContext context) : base(context)
        {
        }
    }

}
