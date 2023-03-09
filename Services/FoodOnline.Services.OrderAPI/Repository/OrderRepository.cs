using FoodOnline.Services.OrderAPI.Models;
using FoodOnline.Services.OrderAPI.Repository.Interfaces;
using FoodOnline.Services.ShopingOrderAPI.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<bool> AddOrderAsync(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationDbContext(_dbContextOptions);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatusAsync(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContextOptions);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(x => x.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
        }
    }
}
