using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ecom.Infrastructure.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;

		public BasketRepository(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}
		public async Task<bool> DeleteBasketAsync(string basketId)
		{
			return await _database.KeyDeleteAsync(basketId);
		}

		public async Task<CustomerBasket> GetBasketAsync(string basketId)
		{
			var data = await _database.StringGetAsync(basketId);
			return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
		}

		public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
		{
			var _basket = await _database.StringSetAsync(customerBasket.Id,
				JsonSerializer.Serialize(customerBasket), TimeSpan.FromDays(30));
			if (!_basket) return null;
			return await GetBasketAsync(customerBasket.Id);
		}
	}
}
