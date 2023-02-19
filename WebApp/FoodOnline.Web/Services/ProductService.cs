using FoodOnline.Web.Common;
using FoodOnline.Web.Common.Enums;
using FoodOnline.Web.Models;
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

        public async Task<T> CreateProductAsync<T>(ProductDto product)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = product,
                Url = Constants.ProductAPIBase + "/api/products",
                AccessToken = string.Empty
            });
        }

        public async Task<T> DeleteProductAsync<T>(int productId)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.DELETE,
                Url = Constants.ProductAPIBase + "/api/products/" + productId,
                AccessToken = string.Empty
            });
        }

        public async Task<T> GetAllProductsAsync<T>()
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = Constants.ProductAPIBase + "/api/products",
                AccessToken = string.Empty
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int productId)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = Constants.ProductAPIBase + "/api/products/" + productId,
                AccessToken = string.Empty
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto product)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.PUT,
                Data = product,
                Url = Constants.ProductAPIBase + "/api/products",
                AccessToken = string.Empty
            });
        }
    }
}
