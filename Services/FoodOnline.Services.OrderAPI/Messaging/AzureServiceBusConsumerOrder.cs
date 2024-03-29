﻿using Azure.Messaging.ServiceBus;
using FoodOnline.MessageBus.Interfaces;
using FoodOnline.Services.OrderAPI.Messages;
using FoodOnline.Services.OrderAPI.Messaging.Interfaces;
using FoodOnline.Services.OrderAPI.Models;
using FoodOnline.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace FoodOnline.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumerOrder : IAzureServiceBusConsumerOrder
    {
        private readonly OrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;

        private readonly string _subscriptionFoodOnline;
        //private readonly string _topicCheckoutMessage;
        private readonly string _queueCheckoutMessage;
        private readonly string _topicOrderPaymentProcess;
        private readonly string _topicUpdatePaymentResult;
        private readonly string _serviceBusConnectionString;

        private ServiceBusProcessor _checkoutProcessor;
        private ServiceBusProcessor _updatePaymentStatusProcessor;

        public AzureServiceBusConsumerOrder(OrderRepository orderRepository,
            IConfiguration configuration,
            IMessageBus messageBus)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            _messageBus = messageBus;

            _serviceBusConnectionString = _configuration.GetValue<string>("Azure:ServiceBus:ConnectionString");            
            _queueCheckoutMessage = _configuration.GetValue<string>("Azure:ServiceBus:QueueCheckoutMessage");            
            _subscriptionFoodOnline = _configuration.GetValue<string>("Azure:ServiceBus:SubscriptionFoodOnline");
            _topicOrderPaymentProcess = _configuration.GetValue<string>("Azure:ServiceBus:TopicOrderPaymentProcess");
            _topicUpdatePaymentResult = _configuration.GetValue<string>("Azure:ServiceBus:TopicUpdatePaymentResult");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            _checkoutProcessor = client.CreateProcessor(_queueCheckoutMessage);
            _updatePaymentStatusProcessor = client.CreateProcessor(_topicUpdatePaymentResult, _subscriptionFoodOnline);
        }

        public async Task StartAsync()
        {
            _checkoutProcessor.ProcessMessageAsync += OnCheckOutMessageReceivedAsync;
            _checkoutProcessor.ProcessErrorAsync += ErrorHandler;
            await _checkoutProcessor.StartProcessingAsync();

            _updatePaymentStatusProcessor.ProcessMessageAsync += OnPaymentUpdateReceivedAsync;
            _updatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await _updatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task StopAsync()
        {
            await _checkoutProcessor.StopProcessingAsync();
            await _checkoutProcessor.DisposeAsync();

            await _updatePaymentStatusProcessor.StopProcessingAsync();
            await _updatePaymentStatusProcessor.DisposeAsync();
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

            // Process Payment request
            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CardCVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
                Email= orderHeader.Email,
            };

            try
            {
                await _messageBus.PublishMessageAsync(paymentRequestMessage, _topicOrderPaymentProcess);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnPaymentUpdateReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            await _orderRepository.UpdateOrderPaymentStatusAsync(paymentResultMessage.OrderId, paymentResultMessage.Status);
            await args.CompleteMessageAsync(args.Message);
        }
    }
}
