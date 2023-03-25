using FoodOnline.Services.ShopingCartAPI.Models.Dtos.Coupon;

namespace FoodOnline.Services.ShopingCartAPI.Repository.Interfaces
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponAsync(string couponName);
    }
}
