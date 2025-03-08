using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Base;

public interface IBaseRepository<T> where T : BaseEntity
{
	Task<List<T>> GetAllAsync();
	Task<T> GetByIdAsync(int id);
	//Task<T> Get(Expression<Func<T>>)
	//GetPaginatedData(int size, int page)
	Task CreateAsync(T data);
	void Update(T data);
	void Delete(T data);
}
