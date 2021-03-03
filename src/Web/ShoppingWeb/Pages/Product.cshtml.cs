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
        private readonly IProductApi _productApi;
        private readonly IBasketApi _cartApi;
        public ProductModel(IProductApi productApi, IBasketApi cartApi)
        {
            _productApi = productApi ?? throw new ArgumentNullException(nameof(productApi));
            _cartApi = cartApi ?? throw new ArgumentNullException(nameof(cartApi));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {

            if (!string.IsNullOrEmpty(categoryName))
            {
                ProductList = await _productApi.GetProductByCategory(categoryName);
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = await _productApi.GetProducts();
                CategoryList = ProductList.Select(p => p.Category).Distinct();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            var product = await _productApi.GetProduct(productId);
            var item = new CartItem
            {
                ProductId = product.Id,
                Price = product.Price,
                ProductName = product.Name,
                Quantity = 1
            };
            await _cartApi.AddItem("test", item);
            return RedirectToPage("Cart");
        }
    }
}