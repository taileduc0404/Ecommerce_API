

namespace Ecom.Core.Entities.Orders
{
	public class Order : BaseEntity<int>
	{
		public Order()
		{

		}
		public Order(string buyerEmail, 
			ShipAddress shipToAddress, 
			DeliveryMethod deliveryMethod, 
			IReadOnlyList<OrderItem> orderItems, 
			decimal subtotal)
		{
			BuyerEmail = buyerEmail;
			ShipToAddress = shipToAddress;
			DeliveryMethod = deliveryMethod;
			OrderItems = orderItems;
			SubTotal = subtotal;
			//PaymentIntentId = paymentIntentId;
		}

		public string BuyerEmail { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public ShipAddress ShipToAddress { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }
		public IReadOnlyList<OrderItem> OrderItems { get; set; }
		public decimal SubTotal { get; set; }
		public string PaymentIntentId { get; set; }
		public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;


		public decimal GetTotal()
		{
			return SubTotal + DeliveryMethod.Price;
		}
	}
}
