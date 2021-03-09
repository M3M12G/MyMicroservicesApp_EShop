using System;

namespace ShoppingWeb.Models
{
    public class PromoGenReqDTO
    {
        public int Quantity { get; set; } = 1;
        public string Title { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(10);
        public double Discount { get; set; } = 0.1;
    }
}
