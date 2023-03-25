namespace FoodOnline.Web.Models.Orders
{
    public class OrderHeaderDto
    {
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime PiclupDateTime { get; set; }
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public string ExpiryMonthYear { get; set; }
    }
}
