using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Models;
using ShoppingWeb.Services_gRPC;

namespace ShoppingWeb.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketApi _basketApi;
        private readonly IOrderingApi _orderApi;
        private readonly PromoCode_gRPC _promoService;
        public CheckOutModel(IBasketApi basketApi, IOrderingApi orderApi, PromoCode_gRPC promoService)
        {
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
            _orderApi = orderApi ?? throw new ArgumentNullException(nameof(orderApi));
            _promoService = promoService ?? throw new ArgumentNullException(nameof(promoService));
        }

        [BindProperty]
        public Order Order { get; set; }
        public Cart Cart { get; set; }
        [BindProperty]
        public string Message { get; set; } = "Get discount Using Promo code";
        [BindProperty]
        public string InputPromo { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketApi.GetCart("test");
            return Page();
        }

        public async Task<IActionResult> OnGetWrongPromo()
        {
            Cart = await _basketApi.GetCart("test");
            Message = "Your Promo Code is Invalid";
            return Page();
        }

        public async Task<IActionResult> OnPostActivatePromoAsync()
        {
            var activated_promo = await _promoService.ActivatePromoCode("test", InputPromo);
            
            if (activated_promo == null)
            {
                return RedirectToPage("CheckOut", "WrongPromo");
            }
            
            var discount = Convert.ToDecimal(activated_promo.Discount);

            var user_basket = await _basketApi.GetCart("test");

            foreach (var item in user_basket.Items)
            {
                var discount_price = item.Price * discount;
                var new_price = item.Price - discount_price;
                item.Price = new_price;
            }

            var updatedCart = await _orderApi.ApplyDiscountRequest(user_basket);

            if (updatedCart == null)
            {
                return RedirectToPage("CheckOut", "WrongPromo");
            }

            return RedirectToPage("Cart");
            
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await _basketApi.GetCart("test");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = "test";
            Order.TotalPrice = Cart.TotalPrice;

            await _orderApi.Checkout(Order);
            await _basketApi.DeleteCart(Cart.Username);
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}