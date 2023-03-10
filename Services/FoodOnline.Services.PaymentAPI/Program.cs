using FoodOnline.MessageBus;
using FoodOnline.MessageBus.Interfaces;
using FoodOnline.PaymentProcessor;
using FoodOnline.PaymentProcessor.Interfaces;
using FoodOnline.Services.PaymentAPI.Extensions;
using FoodOnline.Services.PaymentAPI.Messaging;
using FoodOnline.Services.PaymentAPI.Messaging.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IProcessPayment, ProcessPayment>();
builder.Services.AddSingleton<IAzureServiceBusConsumerPayment, AzureServiceBusConsumerPayment>();
builder.Services.AddSingleton<IMessageBus, AzureServiceMessageBus>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseAzureServiceBusConsumer();

app.Run();
