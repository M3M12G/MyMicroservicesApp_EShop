using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class CartModel : PageModel
    {
        private readonly IBasketApi _api;

        public CartModel(IBasketApi api)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public Cart Cart { get; set; } = new Cart();        

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _api.GetCart("test");           
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string cartItemId)
        {
            Cart = await _api.GetCart("test");
            await _api.DeleteItem(Cart.Username, Cart.Items.Find(i => i.ProductId == cartItemId));
            return RedirectToPage();
        }
    }
}