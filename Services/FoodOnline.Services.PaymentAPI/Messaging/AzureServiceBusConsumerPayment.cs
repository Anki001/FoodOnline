using Azure.Messaging.ServiceBus;
using FoodOnline.MessageBus.Interfaces;
using FoodOnline.PaymentProcessor.Interfaces;
using FoodOnline.Services.PaymentAPI.Messages;
using FoodOnline.Services.PaymentAPI.Messaging.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace FoodOnline.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumerPayment : IAzureServiceBusConsumerPayment
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        private readonly IProcessPayment _processPayment;

        private readonly string _subscriptionOrderPayment;
        private readonly string _topicOrderPaymentProcess;
        private readonly string _topicUpdatePaymentResult;
        private readonly string _serviceBusConnectionString;

        private ServiceBusProcessor _orderPaymentProcessor;

        public AzureServiceBusConsumerPayment(IConfiguration configuration,
            IMessageBus messageBus,
            IProcessPayment processPayment)
        {
            _configuration = configuration;
            _messageBus = messageBus;
            _processPayment = processPayment;

            _serviceBusConnectionString = _configuration.GetValue<string>("Azure:ServiceBus:ConnectionString");
            _subscriptionOrderPayment = _configuration.GetValue<string>("Azure:ServiceBus:SubscriptionOrderPaymentProcess");
            _topicOrderPaymentProcess = _configuration.GetValue<string>("Azure:ServiceBus:TopicOrderPaymentProcess");
            _topicUpdatePaymentResult = _configuration.GetValue<string>("Azure:ServiceBus:TopicUpdatePaymentResult");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            _orderPaymentProcessor = client.CreateProcessor(_topicOrderPaymentProcess, _subscriptionOrderPayment);
        }

        public async Task StartAsync()
        {
            _orderPaymentProcessor.ProcessMessageAsync += ProcessPaymentAsync;
            _orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderPaymentProcessor.StartProcessingAsync();
        }

        public async Task StopAsync()
        {
            await _orderPaymentProcessor.StopProcessingAsync();
            await _orderPaymentProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessPaymentAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentrequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentrequestMessage.OrderId,
                Email = paymentrequestMessage.Email
            };

            try
            {
                await _messageBus.PublishMessageAsync(updatePaymentResultMessage, _topicUpdatePaymentResult);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
