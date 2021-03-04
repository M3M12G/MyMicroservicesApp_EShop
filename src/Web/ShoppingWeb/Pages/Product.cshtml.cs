using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogApi _catalogApi;
        private readonly IBasketApi _basketApi;

        public ProductModel(ICatalogApi catalogApi, IBasketApi basketApi)
        {
            _catalogApi = catalogApi ?? throw new ArgumentNullException(nameof(catalogApi));
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                ProductList = await _catalogApi.GetProductByCategory(categoryName);
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = await _catalogApi.GetProducts();
                CategoryList = ProductList.Select(c => c.Category).Distinct().ToList();
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            var product = await _catalogApi.GetProduct(productId);
            var item = new CartItem()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = 1,
                Price = product.Price,
                Color = "Red"
            };

            await _basketApi.AddItem("test", item);
            return RedirectToPage("Cart");
        }
    }
}