namespace Ecom.Core.Entities.Orders
{
	public class OrderItem : BaseEntity<int>
	{
        public ProductItemOrdered ProductItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}