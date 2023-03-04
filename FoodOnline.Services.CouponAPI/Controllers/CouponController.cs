using FoodOnline.Services.CouponAPI.Models.Dtos;
using FoodOnline.Services.CouponAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _response;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            _response = new ResponseDto();
        }

        [HttpGet("{code}")]
        public async Task<object> GetDiscountForCoupon(string code)
        {
            try
            {
                CouponDto couponDto = await _couponRepository.GetCouponByCode(code);
                _response.Result = couponDto;
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
