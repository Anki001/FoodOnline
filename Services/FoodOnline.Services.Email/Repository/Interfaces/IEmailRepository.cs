using FoodOnline.Services.Email.Messages;

namespace FoodOnline.Services.Email.Repository.Interfaces
{
    public interface IEmailRepository
    {        
        Task SendLogEmailAsync(UpdatePaymentResultMessage message);
    }
}
