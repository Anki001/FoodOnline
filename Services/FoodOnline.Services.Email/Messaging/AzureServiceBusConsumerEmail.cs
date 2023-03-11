using Azure.Messaging.ServiceBus;
using FoodOnline.MessageBus.Interfaces;
using FoodOnline.Services.Email.Messages;
using FoodOnline.Services.Email.Messaging.Interfaces;
using FoodOnline.Services.Email.Models;
using FoodOnline.Services.Email.Repository;
using Newtonsoft.Json;
using System.Text;

namespace FoodOnline.Services.Email.Messaging
{
    public class AzureServiceBusConsumerEmail : IAzureServiceBusConsumerEmail
    {
        private readonly EmailRepository _emailRepository;
        private readonly IConfiguration _configuration;

        private readonly string _subscriptionEmail;
        private readonly string _topicUpdatePaymentResult;
        private readonly string _serviceBusConnectionString;

        private ServiceBusProcessor _updatePaymentStatusProcessor;

        public AzureServiceBusConsumerEmail(EmailRepository emailRepository,
            IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;

            _serviceBusConnectionString = _configuration.GetValue<string>("Azure:ServiceBus:ConnectionString");
            _subscriptionEmail = _configuration.GetValue<string>("Azure:ServiceBus:SubscriptionEmail");
            _topicUpdatePaymentResult = _configuration.GetValue<string>("Azure:ServiceBus:TopicUpdatePaymentResult");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            _updatePaymentStatusProcessor = client.CreateProcessor(_topicUpdatePaymentResult, _subscriptionEmail);
        }

        public async Task StartAsync()
        {
            _updatePaymentStatusProcessor.ProcessMessageAsync += OnPaymentUpdateReceivedAsync;
            _updatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await _updatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task StopAsync()
        {
            await _updatePaymentStatusProcessor.StopProcessingAsync();
            await _updatePaymentStatusProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnPaymentUpdateReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage updatePaymentResult = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            try
            {
                await _emailRepository.SendLogEmailAsync(updatePaymentResult);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
