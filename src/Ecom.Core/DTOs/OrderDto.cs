
using Ecom.Core.Entities.Orders;

namespace Ecom.Core.DTOs
{
	public class OrderDto
	{
		public string basketId { get; set; }
		public int deliveryMethodId { get; set; }
		public AddressDto shipToAddress { get; set; }
	}
}
