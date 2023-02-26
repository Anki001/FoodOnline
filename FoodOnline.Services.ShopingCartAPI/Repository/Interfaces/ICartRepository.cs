using FoodOnline.Services.ShopingCartAPI.Models.Dtos;

namespace FoodOnline.Services.ShopingCartAPI.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserIdAsync(string userId);
        Task<CartDto> CreateUpdateCartAsync(CartDto cartDto);
        Task<bool> RemoveFromCartAsync(int cartDetailsId);
        Task<bool> ClearCartAsync(string userId);
    }
}
