using AutoMapper;
using FoodOnline.Services.CouponAPI.Models;
using FoodOnline.Services.CouponAPI.Models.Dtos;

namespace FoodOnline.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();                
            });

            return mappingConfig;
        }
    }
}
