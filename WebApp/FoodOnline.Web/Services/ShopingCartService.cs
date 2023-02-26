using FoodOnline.Web.Common;
using FoodOnline.Web.Common.Enums;
using FoodOnline.Web.Models;
using FoodOnline.Web.Models.Cart;
using FoodOnline.Web.Services.Interfaces;

namespace FoodOnline.Web.Services
{
    public class ShopingCartService : BaseService, IShopingCartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ShopingCartService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = Constants.ShopingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(int userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = Constants.ShopingCartAPIBase + "/api/cart/GetCart/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartId,
                Url = Constants.ShopingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = Constants.ShopingCartAPIBase + "/api/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}
