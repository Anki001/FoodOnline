using AutoMapper;
using FoodOnline.Services.ProductAPI.Models;
using FoodOnline.Services.ProductAPI.Models.Dtos;

namespace FoodOnline.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
            });

            return mappingConfig;
        }
    }
}
