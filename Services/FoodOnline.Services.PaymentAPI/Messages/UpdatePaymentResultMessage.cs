using FoodOnline.MessageBus;

namespace FoodOnline.Services.PaymentAPI.Messages
{
    public class UpdatePaymentResultMessage: BaseMessage
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
    }
}
