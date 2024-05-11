using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Orders;

namespace Ecom.API.MappingProfiles
{
	public class OrderMapping : Profile
	{
		public OrderMapping()
		{
			CreateMap<ShipAddress, AddressDto>().ReverseMap();
			CreateMap<Order, OrderToReturnDto>().ReverseMap();
			CreateMap<OrderItem, OrderItemDto>().ReverseMap();
		}
	}
}
