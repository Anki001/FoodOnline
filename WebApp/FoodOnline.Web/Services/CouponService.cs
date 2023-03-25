using FoodOnline.Web.Common;
using FoodOnline.Web.Common.Enums;
using FoodOnline.Web.Models;
using FoodOnline.Web.Services.Interfaces;

namespace FoodOnline.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetCouponDetailsAsync<T>(string couponCode, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,                
                Url = Constants.CouponApiBase + "/api/coupon/" + couponCode,
                AccessToken = token
            });
        }
    }
}
