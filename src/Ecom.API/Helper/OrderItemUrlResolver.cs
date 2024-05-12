using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;

namespace Ecom.API.Helper
{
	public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl))
			{
				return _configuration["ApiURL"] + source.ProductItemOrdered.PictureUrl;
			}
			return null;
		}
	}
}
