using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Repositories
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ApplicationDbContext _context;

		public OrderService(IUnitOfWork unitOfWork, ApplicationDbContext context)
		{
			_unitOfWork = unitOfWork;
			_context = context;
		}

		public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress)
		{
			// get basket item
			var basket = await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
			var items = new List<OrderItem>();

			// fill item
			//Parallel.ForEach(basket.BasketItem, async item =>
			//{
			//	var productItem = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
			//	var productItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.ProductPicture);
			//	var orderItem = new OrderItem(productItemOrdered, item.Price, item.Quantity);
			//	lock (items)
			//	{
			//		items.Add(orderItem);
			//	}
			//});

			foreach (var item in basket.BasketItem)
			{
				var productItem = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
				var productItemOrderd = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.ProductPicture);
				var orderItem = new OrderItem(productItemOrderd, item.Price, item.Quantity);

				items.Add(orderItem);
			}
			await _context.orderItems.AddRangeAsync(items);
			await _context.SaveChangesAsync();


			// get delivery method
			var deliveryMethod = await _context.deliveryMethods.FirstOrDefaultAsync(x => x.Id == deliveryMethodId);

			// calculate subtotal
			var subTotal = items.Sum(x => x.Price * x.Quantity);

			// initialize in ctor
			var order = new Order(buyerEmail, shipAddress, deliveryMethod, items, subTotal);

			// check order is not null
			if (order is null)
			{
				return null;
			}

			// adding order in db
			await _context.orders.AddAsync(order);
			await _context.SaveChangesAsync();

			// remove basket
			//await _unitOfWork.BasketRepository.DeleteBasketAsync(basketId);
			return order;
		}
		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		=> await _context.deliveryMethods.ToListAsync();

		public Task<Order> GetOrderById(int id, string buyerEmail)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
