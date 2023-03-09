using FoodOnline.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.ShopingOrderAPI.DBContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }        
    }
}
