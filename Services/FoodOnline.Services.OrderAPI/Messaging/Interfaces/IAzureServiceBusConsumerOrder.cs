namespace FoodOnline.Services.OrderAPI.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumerOrder
    {
        Task StartAsync();
        Task StopAsync();
    }
}
