﻿namespace FoodOnline.Services.OrderAPI.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public string ExpiryMonthYear { get; set; }
        public int CartTotalItem { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
