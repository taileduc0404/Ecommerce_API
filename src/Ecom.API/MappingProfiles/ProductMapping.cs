using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities;

namespace Ecom.API.MappingProfiles
{
	public class ProductMapping : Profile
	{
		public ProductMapping()
		{
			CreateMap<Product, ProductDto>()
				.ForMember(x => x.CategoryName, a => a.MapFrom(s => s.Category.Name))
				.ForMember(x => x.ProductPicture, a => a.MapFrom<ProductUrlResolver>())
				.ReverseMap();
			CreateMap<AddProductDto, Product>().ReverseMap();
			CreateMap<Product, UpdateProductDto>().ReverseMap();
		}
	}
}
