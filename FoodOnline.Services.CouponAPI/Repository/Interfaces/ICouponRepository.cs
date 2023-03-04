using FoodOnline.Services.CouponAPI.Models.Dtos;

namespace FoodOnline.Services.CouponAPI.Repository.Interfaces
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
