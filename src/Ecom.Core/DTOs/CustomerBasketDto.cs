using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.DTOs
{
	public class CustomerBasketDto
	{
		[Required]
		public string Id { get; set; }
		public List<BasketItemDto> BasketItem { get; set; } = new List<BasketItemDto>();
		public int? DeliveryMethodId { get; set; }
		public string ClientSecret { get; set; }
		public string PaymentIntentId { get; set; }
	}
}
