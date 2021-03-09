using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.Discount_gRPC_Services;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketApi _basketApi;
        private readonly IOrderingApi _orderApi;
        private readonly DiscountPromogRPCService _promoService;
        public CheckOutModel(IBasketApi basketApi, IOrderingApi orderApi, DiscountPromogRPCService promoService)
        {
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
            _orderApi = orderApi ?? throw new ArgumentNullException(nameof(orderApi));
            _promoService = promoService ?? throw new ArgumentNullException(nameof(promoService));
        }

        [BindProperty]
        public Order Order { get; set; }
        public Cart Cart { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketApi.GetCart("test");
            return Page();
        }

        public async Task<IActionResult> OnPostActivatePromoAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(await _promoService.ActivatePromoCode("test", Code))
            {
                Message = "Activated";
            }

            Message = "Promocode is invalid";
            return Page();
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