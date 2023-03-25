using FoodOnline.Web.Models.Cart;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodOnline.Web.Services.Interfaces
{
    public interface ICouponService
    {
        Task<T> GetCouponDetailsAsync<T>(string couponCode, string token = null);        
    }
}
