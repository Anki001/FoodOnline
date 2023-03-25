using FoodOnline.Web.Models;
using FoodOnline.Web.Models.Cart;
using FoodOnline.Web.Models.Product;
using FoodOnline.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FoodOnline.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IShopingCartService _shopingCartService;

        public HomeController(ILogger<HomeController> logger,
            IProductService productService,
            IShopingCartService shopingCartService)
        {
            _logger = logger;
            _productService = productService;
            _shopingCartService = shopingCartService;
        }

        public async Task<IActionResult> Index()
        {            
            List<ProductDto> productList = new();
            var response = await _productService.GetAllProductsAsync<ResponseDto>(string.Empty);

            if (response is not null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(productList);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto product = new();
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, string.Empty);

            if (response is not null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDto productDto)
        {
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetailsDto = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };
            var res = await _productService.GetProductByIdAsync<ResponseDto>(productDto.ProductId, "");

            if (res is not null && res.IsSuccess)
            {
                cartDetailsDto.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(res.Result));
            }
            List<CartDetailsDto> cartDetailsDtos = new();
            cartDetailsDtos.Add(cartDetailsDto);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResponse = await _shopingCartService.AddToCartAsync<ResponseDto>(cartDto, accessToken);

            if (addToCartResponse is not null && addToCartResponse.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}