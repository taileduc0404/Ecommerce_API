
using Ecom.Core.Entities.Orders;

namespace Ecom.Core.DTOs
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; }
		public DateTime OrderDate { get; set; }
		public ShipAddress ShipToAddress { get; set; }
		public string DeliveryMethod { get; set; }
		public decimal ShippingPrice { get; set; }
		public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
		public decimal SubTotal { get; set; }
		public decimal Total { get; set; }
		public string OrderStatus { get; set; }
	}
}
