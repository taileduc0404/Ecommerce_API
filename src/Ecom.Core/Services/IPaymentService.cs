using Ecom.Core.Entities;

namespace Ecom.Core.Services
{
	public interface IPaymentService
	{
		Task<CustomerBasket> CreateOrUpdatePayment(string basketId);
	}
}
