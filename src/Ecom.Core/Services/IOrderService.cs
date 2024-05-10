using Ecom.Core.Entities.Orders;

namespace Ecom.Core.Services
{
	public interface IOrderService
	{
		Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress);
		Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail);
		Task<Order> GetOrderById(int id, string buyerEmail);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();	
	}
}
