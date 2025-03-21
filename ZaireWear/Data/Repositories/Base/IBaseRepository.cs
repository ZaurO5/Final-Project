using Core.Entities;

namespace Data.Repositories.Base;

public interface IBaseRepository<T> where T : BaseEntity
{
	Task<List<T>> GetAllAsync();
	Task<T> GetByIdAsync(int id);
	Task CreateAsync(T data);
	void Update(T data);
	void Delete(T data);
}
