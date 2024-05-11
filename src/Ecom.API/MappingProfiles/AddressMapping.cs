using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Entities.Orders;

namespace Ecom.API.MappingProfiles
{
	public class AddressMapping : Profile
	{
		public AddressMapping()
		{
			CreateMap<Address, AddressDto>().ReverseMap();
		}
	}
}
