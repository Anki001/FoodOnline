using Azure.Messaging.ServiceBus;
using FoodOnline.Services.OrderAPI.Messages;
using FoodOnline.Services.OrderAPI.Messaging.Interfaces;
using FoodOnline.Services.OrderAPI.Models;
using FoodOnline.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace FoodOnline.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        private readonly string _subscriptionCheckout;
        private readonly string _topicCheckoutMessage;
        private readonly string _serviceBusConnectionString;

        private ServiceBusProcessor _checkoutProcessor;

        public AzureServiceBusConsumer(OrderRepository orderRepository,
            IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;

            _serviceBusConnectionString = _configuration.GetValue<string>("AzureServiceBus:ConnectionString");
            _topicCheckoutMessage = _configuration.GetValue<string>("AzureServiceBus:TopicCheckoutMessage");
            _subscriptionCheckout = _configuration.GetValue<string>("AzureServiceBus:SubscriptionCheckout");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            _checkoutProcessor = client.CreateProcessor(_topicCheckoutMessage, _subscriptionCheckout);
        }

        public async Task StartAsync()
        {
            _checkoutProcessor.ProcessMessageAsync += OnCheckOutMessageReceivedAsync;
            _checkoutProcessor.ProcessErrorAsync += ErrorHandler;
            await _checkoutProcessor.StartProcessingAsync();
        }

        public async Task StopAsync()
        {
            await _checkoutProcessor.StopProcessingAsync();
            await _checkoutProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckOutMessageReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CardCVV = checkoutHeaderDto.CardCVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderDateTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickupDateTime = checkoutHeaderDto.PickupDateTime
            };

            foreach (var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItem += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrderAsync(orderHeader);
        }
    }
}
