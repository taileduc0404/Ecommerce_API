using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;

namespace Ecom.API.MappingProfiles
{
	public class BasketMapping : Profile
	{
		public BasketMapping()
		{
			CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
			CreateMap<BasketItem, BasketItemDto>().ReverseMap();
		}
	}
}
