using FoodOnline.Services.ProductAPI.Models.Dtos;

namespace FoodOnline.Services.ProductAPI.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateUpdateProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int productId);
    }
}
