using FoodOnline.Services.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Services.Email.DBContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<EmailLog> EmailLogs { get; set; }
    }
}
