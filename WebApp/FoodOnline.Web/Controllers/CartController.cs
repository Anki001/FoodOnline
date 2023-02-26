using FoodOnline.Web.Models;
using FoodOnline.Web.Models.Cart;
using FoodOnline.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodOnline.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShopingCartService _shopingCartService;

        public CartController(IProductService productService,
            IShopingCartService shopingCartService)
        {
            _productService = productService;
            _shopingCartService = shopingCartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await GetCartDtoForLoggedinUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shopingCartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDto> GetCartDtoForLoggedinUser()
        {
            var userId = User.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shopingCartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

            CartDto cartDto = new();
            if (response is not null && response.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }

            if (cartDto.CartHeader is not null)
            {
                foreach (var details in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += details.Product.Price * details.Count;
                }
            }

            return cartDto;
        }
    }
}
