using FoodOnline.Services.ShopingCartAPI.Models.Dtos;
using FoodOnline.Services.ShopingCartAPI.Models.Dtos.Coupon;
using FoodOnline.Services.ShopingCartAPI.Repository.Interfaces;
using Newtonsoft.Json;

namespace FoodOnline.Services.ShopingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;

        public CouponRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponDto> GetCouponAsync(string couponName)
        {
            var response = await _client.GetAsync($"/api/coupon/{couponName}");

            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
