using Azure.Messaging.ServiceBus;
using FoodOnline.MessageBus.Interfaces;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FoodOnline.MessageBus
{
    public class AzureServiceMessageBus : IMessageBus
    {
        // It can be moved to appsetting.json
        string connectionString = "Endpoint=sb://foodonline.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gQIZDVgDA5K2tCHe+pHgNqWQeH08+0YhH+ASbCKCAhQ=";
        public async Task PublishMessageAsync(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
        }
    }
}
