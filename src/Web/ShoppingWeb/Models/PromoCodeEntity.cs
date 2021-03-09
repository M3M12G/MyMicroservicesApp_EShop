using System;

namespace ShoppingWeb.Models
{
    public class PromoCodeEntity
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string? Username { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Discount { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
