using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; } // Foreign Key
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; } // Navigation properties
        public int ProductId { get; set; }        
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
