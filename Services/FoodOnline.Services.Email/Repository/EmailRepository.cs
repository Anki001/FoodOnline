using FoodOnline.Services.Email.Models;
using FoodOnline.Services.Email.Repository.Interfaces;
using FoodOnline.Services.Email.DBContexts;
using Microsoft.EntityFrameworkCore;
using FoodOnline.Services.Email.Messages;

namespace FoodOnline.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public EmailRepository(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task SendLogEmailAsync(UpdatePaymentResultMessage message)
        {
            // Implement an email sender or call some other class library
            EmailLog emailLog = new()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully."
            };

            await using var _db = new ApplicationDbContext(_dbContextOptions);
            _db.EmailLogs.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}
