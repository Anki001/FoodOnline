using AutoMapper;
using FoodOnline.Services.CouponAPI.DBContexts;
using FoodOnline.Services.CouponAPI.Models.Dtos;
using FoodOnline.Services.CouponAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var couponFromDb = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.CouponCode == couponCode);
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
