using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Shared;

namespace Ecom.Core.Interfaces
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		Task<IEnumerable<ProductDto>> GetAll(ProductParams productParams);
		Task<bool> AddAsync(AddProductDto dto);
		Task<bool> UpdateAsync(int id, UpdateProductDto dto);
		Task<bool> DeleteAsyncWithPicture(int id);
	}
}
