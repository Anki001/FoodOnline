using AutoMapper;
using FoodOnline.Services.ProductAPI.DBContexts;
using FoodOnline.Services.ProductAPI.Models;
using FoodOnline.Services.ProductAPI.Models.Dtos;
using FoodOnline.Services.ProductAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<ProductDto, Product>(productDto);

            if (product.ProductId > 0)
            {
                _dbContext.Update(product);
            }
            else
            {
                _dbContext.Add(product);
            }
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (product == null)
                {
                    return false;
                }
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _dbContext.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
