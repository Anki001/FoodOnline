using FoodOnline.Web.Common;
using FoodOnline.Web.Common.Enums;
using FoodOnline.Web.Models;
using FoodOnline.Web.Models.Product;
using FoodOnline.Web.Services.Interfaces;

namespace FoodOnline.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> CreateProductAsync<T>(ProductDto product, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = product,
                Url = Constants.ProductApiBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> DeleteProductAsync<T>(int productId, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.DELETE,
                Url = Constants.ProductApiBase + "/api/products/" + productId,
                AccessToken = token
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = Constants.ProductApiBase + "/api/products",
                AccessToken = token
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int productId, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = Constants.ProductApiBase + "/api/products/" + productId,
                AccessToken = token
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto product, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.PUT,
                Data = product,
                Url = Constants.ProductApiBase + "/api/products",
                AccessToken = token
            });
        }
    }
}
