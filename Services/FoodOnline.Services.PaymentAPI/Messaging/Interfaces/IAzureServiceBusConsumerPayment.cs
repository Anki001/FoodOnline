namespace FoodOnline.Services.PaymentAPI.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumerPayment
    {
        Task StartAsync();
        Task StopAsync();
    }
}
