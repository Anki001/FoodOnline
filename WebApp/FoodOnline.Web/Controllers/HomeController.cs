using FoodOnline.Web.Models;
using FoodOnline.Web.Services.Interfaces;
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

        public HomeController(ILogger<HomeController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
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