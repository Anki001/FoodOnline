using FoodOnline.Services.OrderAPI.Models;

namespace FoodOnline.Services.OrderAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool>AddOrderAsync(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatusAsync(int orderHeaderId, bool paid);
    }
}
