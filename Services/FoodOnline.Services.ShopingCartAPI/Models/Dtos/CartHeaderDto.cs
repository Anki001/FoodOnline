using System.ComponentModel.DataAnnotations;

namespace FoodOnline.Services.ShopingCartAPI.Models.Dtos
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
    }
}
