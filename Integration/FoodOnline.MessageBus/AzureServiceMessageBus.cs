using Azure.Messaging.ServiceBus;
using FoodOnline.MessageBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FoodOnline.MessageBus
{
    public class AzureServiceMessageBus : IMessageBus
    {
        // It can be moved to appsetting.json
        private readonly IConfiguration _configuration;
        public AzureServiceMessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }        
        
        public async Task PublishMessageAsync(BaseMessage message, string topicName)
        {
            var serviceBuConnectionString = _configuration.GetValue<string>("Azure:ServiceBus:ConnectionString");

            await using var client = new ServiceBusClient(serviceBuConnectionString);

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
