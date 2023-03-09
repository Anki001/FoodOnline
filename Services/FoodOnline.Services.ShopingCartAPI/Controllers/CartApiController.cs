using FoodOnline.MessageBus.Interfaces;
using FoodOnline.Services.ShopingCartAPI.Models.Dtos;
using FoodOnline.Services.ShopingCartAPI.Models.Dtos.Coupon;
using FoodOnline.Services.ShopingCartAPI.Models.Messages;
using FoodOnline.Services.ShopingCartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.Services.ShopingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartApiController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        protected ResponseDto _response;

        public CartApiController(ICartRepository cartRepository,
            ICouponRepository couponRepository,
            IMessageBus messageBus,
            IConfiguration configuration)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserIdAsync(userId);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart([FromBody] CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartRepository.CreateUpdateCartAsync(cartDto);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart([FromBody] CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartRepository.CreateUpdateCartAsync(cartDto);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                var isCartRemoved = await _cartRepository.RemoveFromCartAsync(cartDetailsId);
                _response.Result = isCartRemoved;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var isCouponApply = await _cartRepository.ApplyCouponeAsync(cartDto.CartHeader.UserId,
                    cartDto.CartHeader.CouponCode);

                _response.Result = isCouponApply;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                var isCouponRemoved = await _cartRepository.RemoveCouponAsync(userId);
                _response.Result = isCouponRemoved;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserIdAsync(checkoutHeader.UserId);
                if (cartDto is null)
                    return BadRequest();

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCouponAsync(checkoutHeader.CouponCode);
                    if (checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string> { "Coupon price has changed, please confirm" };
                        _response.DisplayMessage = "Coupon price has changed, please confirm";
                        return _response;
                    }
                }

                checkoutHeader.CartDetails = cartDto.CartDetails;
                //logic to add message to process order
                var checkoutTopicName = _configuration.GetValue<string>("Azure:ServiceBus:CheckoutTopic");
                await _messageBus.PublishMessageAsync(checkoutHeader, checkoutTopicName);
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
