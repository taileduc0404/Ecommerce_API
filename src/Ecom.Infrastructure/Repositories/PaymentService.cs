using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Ecom.Infrastructure.Repositories
{
	public class PaymentService : IPaymentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;

		public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, ApplicationDbContext context)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
			_context = context;
		}

		public async Task<CustomerBasket> CreateOrUpdatePayment(string basketId)
		{
			StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
			var basket = await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
			var shippingPrice = 0m;

			if (basket == null) { return null; }
			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await _context.deliveryMethods.Where(x => x.Id == basket.DeliveryMethodId.Value)
					.FirstOrDefaultAsync();
				shippingPrice = deliveryMethod.Price;
			}

			foreach (var item in basket.BasketItem)
			{
				var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
				if (item.Price != product.Price)
				{
					item.Price = product.Price;
				}
			}

			var service = new PaymentIntentService();
			PaymentIntent intent;
			if (string.IsNullOrEmpty(basket.PaymentIntentId))
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)basket.BasketItem.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
					Currency = "USD",
					PaymentMethodTypes = new List<string> { "card" }
				};
				intent = await service.CreateAsync(options);
				basket.PaymentIntentId = intent.Id;
				basket.ClientSecret = intent.ClientSecret;
			}
			else
			{
				// Update
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)basket.BasketItem.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
					Currency = "USD",
					PaymentMethodTypes = new List<string> { "Card" }
				};
				await service.UpdateAsync(basket.PaymentIntentId, options);
			}
			await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
			return basket;
		}
	}
}
