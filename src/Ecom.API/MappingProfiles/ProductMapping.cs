using AutoMapper;
using Ecom.API.DTOs;
using Ecom.Core.Entities;

namespace Ecom.API.MappingProfiles
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.CategoryName, a => a.MapFrom(s => s.Category.Name))
                .ReverseMap();
            CreateMap<AddProductDto, Product>().ReverseMap();
        }
    }
}
