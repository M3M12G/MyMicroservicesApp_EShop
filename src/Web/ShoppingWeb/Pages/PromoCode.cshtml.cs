using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWeb.Discount_gRPC_Services;
using ShoppingWeb.Models;

namespace ShoppingWeb.Pages
{
    public class PromoCodeModel : PageModel
    {
        private readonly DiscountPromogRPCService _promoService;

        public PromoCodeModel(DiscountPromogRPCService promoService)
        {
            _promoService = promoService ?? throw new ArgumentNullException(nameof(promoService));
        }
        public IEnumerable<PromoCodeEntity> PromoCodes { get; set; } = new List<PromoCodeEntity>();

        public async Task<IActionResult> OnGetAsync()
        {
            PromoCodes = await _promoService.GetAllPromoCodes();

            return Page();
        }
    }
}
