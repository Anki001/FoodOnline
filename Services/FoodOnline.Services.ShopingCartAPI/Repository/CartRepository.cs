using AutoMapper;
using FoodOnline.Services.ShopingCartAPI.DBContexts;
using FoodOnline.Services.ShopingCartAPI.Models;
using FoodOnline.Services.ShopingCartAPI.Models.Dtos;
using FoodOnline.Services.ShopingCartAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.ShopingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCouponeAsync(string userId, string couponCode)
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cartFromDb is null) return false;

            cartFromDb.CouponCode = couponCode;
            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCouponAsync(string userId)
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cartFromDb is null) return false;

            cartFromDb.CouponCode = string.Empty;
            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            try
            {
                var cartHeaderFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

                if (cartHeaderFromDb is null) return false;

                _dbContext.CartDetails
                    .RemoveRange(_dbContext.CartDetails.Where(x => x.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _dbContext.Remove(cartHeaderFromDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CartDto> CreateUpdateCartAsync(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);

            // Check if product is exist in database if not create it
            var prodInDb = await _dbContext.Products
                .FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetails.FirstOrDefault()
                .ProductId);

            if (prodInDb is null)
            {
                _dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _dbContext.SaveChangesAsync();
            }

            // Check if header is null
            var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb is null)
            {
                // Create cart header and details
                _dbContext.CartHeaders.Add(cart.CartHeader);
                await _dbContext.SaveChangesAsync();

                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;

                // Already added product of this cartdetails so null
                cart.CartDetails.FirstOrDefault().Product = null;

                _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _dbContext.SaveChangesAsync();

            }
            else
            {
                // If header is not null
                // Check details has same product
                var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                x.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb is null)
                {
                    // create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;

                    // Already added product of this cartdetails so null
                    cart.CartDetails.FirstOrDefault().Product = null;

                    _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    // Already added product of this cartdetails so null
                    cart.CartDetails.FirstOrDefault().Product = null;

                    // Update the count / cart details
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;

                    _dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    _dbContext.SaveChanges();
                }
            }
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId)
            };

            cart.CartDetails = _dbContext.CartDetails
                .Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId).Include(x => x.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCartAsync(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _dbContext.CartDetails
                    .FirstOrDefaultAsync(x => x.CartDetailsId == cartDetailsId);

                var totalCountOfCartItems = _dbContext.CartDetails
                    .Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();

                _dbContext.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeaders
                        .FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);

                    _dbContext.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
