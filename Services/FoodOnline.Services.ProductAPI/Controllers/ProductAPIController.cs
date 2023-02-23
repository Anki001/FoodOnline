using FoodOnline.Services.ProductAPI.Models.Dtos;
using FoodOnline.Services.ProductAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IProductRepository _productRepository;

        public ProductAPIController(IProductRepository productRepository)
        {
            _response = new ResponseDto();
            _productRepository = productRepository;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var productDtos = await _productRepository.GetProductsAsync();
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                var productDtos = await _productRepository.GetProductByIdAsync(id);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> Post([FromBody] ProductDto product)
        {
            try
            {
                var model = await _productRepository.CreateUpdateProductAsync(product);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut]
        [Authorize]
        public async Task<ResponseDto> Put([FromBody] ProductDto product)
        {
            try
            {
                var model = await _productRepository.CreateUpdateProductAsync(product);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var isSucess = await _productRepository.DeleteProductAsync(id);
                _response.Result = isSucess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
