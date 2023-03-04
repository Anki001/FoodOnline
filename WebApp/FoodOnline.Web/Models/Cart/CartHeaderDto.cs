using FoodOnline.Web.Models.Orders;

namespace FoodOnline.Web.Models.Cart
{
    public class CartHeaderDto : OrderHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
    }
}
