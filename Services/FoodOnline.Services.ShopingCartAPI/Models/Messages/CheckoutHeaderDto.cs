using FoodOnline.MessageBus;
using FoodOnline.Services.ShopingCartAPI.Models.Dtos;

namespace FoodOnline.Services.ShopingCartAPI.Models.Messages
{
    public class CheckoutHeaderDto : BaseMessage
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime PiclupDateTime { get; set; }
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public string ExpiryMonthYear { get; set; }
        public int CartTotalItem { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
