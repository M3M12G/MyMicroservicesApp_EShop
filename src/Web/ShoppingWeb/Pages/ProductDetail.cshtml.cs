using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly IProductApi _productApi;
        private readonly IBasketApi _basketApi;

        public ProductDetailModel(IProductApi productApi, IBasketApi basketApi)
        {
            _productApi = productApi ?? throw new ArgumentNullException(nameof(productApi));
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public Product Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return NotFound();
            }
            Product = await _productApi.GetProduct(productId);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            Product = await _productApi.GetProduct(productId);
            var item = new CartItem
            {
                ProductId = productId,
                Quantity = Quantity,
                Color = Color,
                ProductName = Product.Name,
                Price = Product.Price
            };
            await _basketApi.AddItem("test", item);
            return RedirectToPage("Cart");
        }
    }
}