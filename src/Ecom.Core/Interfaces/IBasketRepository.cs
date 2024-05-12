using Ecom.Core.Entities;

namespace Ecom.Core.Interfaces
{
	public interface IBasketRepository
	{
		Task<CustomerBasket> GetBasketAsync(string basketId);
		Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket);
		Task<bool> DeleteBasketAsync(string basketId);
	}
}
