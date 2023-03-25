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
                Url = Constants.ShopingCartApiBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> ApplyCouponAsync<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = Constants.ShopingCartApiBase + "/api/cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> RemoveCouponAsync<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = userId,
                Url = Constants.ShopingCartApiBase + "/api/cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = Constants.ShopingCartApiBase + "/api/cart/GetCart/" + userId,
                AccessToken = token
            });
        }        

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartId,
                Url = Constants.ShopingCartApiBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = Constants.ShopingCartApiBase + "/api/cart/UpdateCart",
                AccessToken = token
            });
        }

        public async Task<T> CheckoutAsync<T>(CartHeaderDto cartHeader, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartHeader,
                Url = Constants.ShopingCartApiBase + "/api/cart/Checkout",
                AccessToken = token
            });
        }
    }
}
