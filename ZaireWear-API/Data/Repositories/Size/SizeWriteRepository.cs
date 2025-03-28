using Data.Contexts;
using Data.Repositories.Base;

namespace Data.Repositories.Size
{
    public class SizeWriteRepository : BaseWriteRepository<Core.Entities.Size>, ISizeWriteRepository
    {
        public SizeWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
