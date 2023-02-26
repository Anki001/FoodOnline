using FoodOnline.Web.Models.Cart;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodOnline.Web.Services.Interfaces
{
    public interface IShopingCartService
    {
        Task<T> GetCartByUserIdAsync<T>(int userId, string token = null);
        Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
    }
}
