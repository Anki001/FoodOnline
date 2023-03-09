namespace FoodOnline.Services.OrderAPI.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumer
    {
        Task StartAsync();
        Task StopAsync();
    }
}
