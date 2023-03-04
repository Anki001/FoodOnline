using FoodOnline.Web.Models;
using FoodOnline.Web.Models.Cart;
using FoodOnline.Web.Models.Coupon;
using FoodOnline.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FoodOnline.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShopingCartService _shopingCartService;
        private readonly ICouponService _couponService;
        public CartController(IProductService productService,
            IShopingCartService shopingCartService,
            ICouponService couponService)
        {
            _productService = productService;
            _shopingCartService = shopingCartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await GetCartDtoForLoggedinUser());
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shopingCartService.ApplyCouponAsync<ResponseDto>(cartDto, accessToken);

            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shopingCartService.RemoveCouponAsync<ResponseDto>(cartDto.CartHeader.UserId, accessToken);

            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
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
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var couponDetails = await _couponService.GetCouponDetailsAsync<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);

                    if (couponDetails is not null && couponDetails.IsSuccess)
                    {
                        var couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponDetails.Result));
                        cartDto.CartHeader.DiscountTotal = couponDto.DiscountAmount;
                    }
                }
                foreach (var details in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += details.Product.Price * details.Count;
                }
                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }

            return cartDto;
        }
    }
}
