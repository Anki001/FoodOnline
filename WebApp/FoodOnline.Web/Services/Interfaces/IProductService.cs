using FoodOnline.Web.Models;

namespace FoodOnline.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int productId);
        Task<T> CreateProductAsync<T>(ProductDto product);
        Task<T> UpdateProductAsync<T>(ProductDto product);
        Task<T> DeleteProductAsync<T>(int productId);
    }
}
