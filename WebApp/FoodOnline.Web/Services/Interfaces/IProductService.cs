using FoodOnline.Web.Models.Product;

namespace FoodOnline.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int productId, string token);
        Task<T> CreateProductAsync<T>(ProductDto product, string token);
        Task<T> UpdateProductAsync<T>(ProductDto product, string token);
        Task<T> DeleteProductAsync<T>(int productId, string token);
    }
}
