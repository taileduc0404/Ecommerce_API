using Ecom.Core.Entities;
using System.Linq.Expressions;

namespace Ecom.Core.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity<int>
	{
		Task<IReadOnlyList<T>> GetAllAsync();
		IEnumerable<T> GetAll();

		Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
		Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

		Task<T> GetAsync(int id);
		Task AddAsync(T entity);
		Task UpdateAsync(T entity, int id);
		Task DeletetAsync(int id);
		Task<int> CountAsync();

	}
}
