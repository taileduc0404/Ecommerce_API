
using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.DTOs
{
	public class BasketItemDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string ProductName { get; set; }

		[Required]
		public string ProductPicture { get; set; }

		[Range(0.1, double.MaxValue)]
		[Required]
		public decimal Price { get; set; }

		[Range(1, int.MaxValue)]
		[Required]
		public int Quantity { get; set; }

		[Required]
		public string Category { get; set; }

	}
}
