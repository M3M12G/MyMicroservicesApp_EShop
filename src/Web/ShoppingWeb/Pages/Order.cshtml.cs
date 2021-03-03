using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class OrderModel : PageModel
    {
        private readonly IOrderingApi _orderApi;

        public OrderModel(IOrderingApi orderApi)
        {
            _orderApi = orderApi ?? throw new ArgumentNullException(nameof(orderApi));
        }

        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        public async Task<IActionResult> OnGetAsync()
        {
            Orders = await _orderApi.GetOrdersByUsername("test");
            return Page();
        }       
    }
}