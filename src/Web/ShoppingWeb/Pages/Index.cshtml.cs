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
        private readonly ICatalogApi _catalogApi;
        private readonly IBasketApi _basketApi;

        public IndexModel(IBasketApi basketApi, ICatalogApi catalogApi)
        {
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
            _catalogApi = catalogApi ?? throw new ArgumentNullException(nameof(catalogApi)); ;
        }

        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogApi.GetProducts();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            var product = await _catalogApi.GetProduct(productId);
            
            var item = new CartItem() {ProductId=product.Id, ProductName=product.Name, Quantity=1, Color="Red", Price=product.Price };
            
            await _basketApi.AddItem("test", item);
            return RedirectToPage("Cart");
        }
    }
}
