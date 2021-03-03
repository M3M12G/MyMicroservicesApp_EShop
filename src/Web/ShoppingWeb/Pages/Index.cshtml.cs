using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductApi _productApi;
        private readonly IBasketApi _basketApi;

        public IndexModel(IProductApi productApi, IBasketApi basketApi)
        {
            _productApi = productApi ?? throw new ArgumentNullException(nameof(productApi));
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _productApi.GetProducts();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            var item = await _productApi.GetProduct(productId);
            await _basketApi.AddItem("test", new CartItem {ProductId = productId, Color = "Black", Price = item.Price,
            Quantity = 1, ProductName = item.Name});
            return RedirectToPage("Cart");
        }
    }
}
