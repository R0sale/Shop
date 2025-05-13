using AutoMapper;
using Entities.Models;
using Shared.DTOObjects;

namespace Inno_Shop
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductForCreationDTO, Product>();
            CreateMap<ProductForUpdateDTO, Product>();
        }
    }
}