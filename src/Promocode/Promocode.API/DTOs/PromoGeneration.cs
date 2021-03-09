using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promocode.API.DTOs
{
    public class PromoGeneration
    {
        public int Quantity { get; set; } = 1;
        public string Title { get; set; }
        public string Target { get; set; } = "all";
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(10);
        public double Discount { get; set; } = 0.1;
    }
}
