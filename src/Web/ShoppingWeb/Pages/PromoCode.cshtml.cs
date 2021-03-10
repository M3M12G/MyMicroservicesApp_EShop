using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShoppingWeb.Models;
using ShoppingWeb.Services_gRPC;

namespace ShoppingWeb.Pages
{
    public class PromoCodeModel : PageModel
    {
        private readonly PromoCode_gRPC _promoService;
        private readonly ILogger<PromoCodeModel> _logger;

        public PromoCodeModel(PromoCode_gRPC promoService, ILogger<PromoCodeModel> logger)
        {
            _promoService = promoService ?? throw new ArgumentNullException(nameof(promoService));
            _logger = logger;
        }
        public IEnumerable<PromoCodeEntity> PromoCodes { get; set; } = new List<PromoCodeEntity>();
        [BindProperty]
        public PromoGenReqDTO genReqDTO { get; set; }

        [BindProperty]
        public string PromoTitle { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                PromoCodes = await _promoService.GetAllValidPromos();
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
            }


            return Page();
        }

        public async Task<IActionResult> OnPostGeneratePromos()
        {
            try
            {
                var calculated = genReqDTO.Discount / 100;
                genReqDTO.Discount = calculated;
                var dateFix = DateTime.SpecifyKind(genReqDTO.ExpirationDate, DateTimeKind.Utc);
                genReqDTO.ExpirationDate = dateFix;
                var gen_Result = await _promoService.GeneratePromoCodes(genReqDTO);

                if (!gen_Result)
                {
                    return RedirectToPage("Index");
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
            }
            return Page();
        }

    }
}
