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
        private readonly IBasketApi _basketApi;

        public CartModel(IBasketApi basketApi)
        {
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
        }

        public Cart Cart { get; set; } = new Cart();        

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketApi.GetCart("test");            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string cartItemId)
        {
            Cart = await _basketApi.GetCart("test");
            await _basketApi.DeleteItem(Cart.Username, Cart.Items.Find(i => i.ProductId == cartItemId));
            return RedirectToPage();
        }
    }
}