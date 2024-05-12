namespace Ecom.Core.DTOs
{
	public class OrderItemDto
	{
		public int ProductItemId { get; set; }
		public string ProductItemName { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}