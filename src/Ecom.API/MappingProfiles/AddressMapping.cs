using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;

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
