using AutoMapper;
using FoodOnline.Services.ShopingCartAPI.Models;
using FoodOnline.Services.ShopingCartAPI.Models.Dtos;

namespace FoodOnline.Services.ShopingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<Cart, CartDto>().ReverseMap();                
            });

            return mappingConfig;
        }
    }
}
