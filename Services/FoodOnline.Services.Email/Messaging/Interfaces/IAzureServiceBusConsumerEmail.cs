namespace FoodOnline.Services.Email.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumerEmail
    {
        Task StartAsync();
        Task StopAsync();
    }
}
