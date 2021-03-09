using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promocode.API.DTOs;
using Promocode.API.Entities;
using Promocode.API.Helpers;
using Promocode.API.Repositories.Interfaces;

namespace Promocode.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IPromocodeRepository _repository;
        private readonly ILogger<PromocodeController> _logger;

        //THIS ENDPOINTS MOSTLY USED BY ADMIN OR USED BY PERSON THAT TAKE CARE OF MARKETING (RELEASING PROMOCODES, SENDING NOTIFICATIONS)
        /*
            GetPromoCodes - Returns all promo codes
            CreatePromocode - Creates just one promocode
            GetPromocodesByTitle - Returns all promo codes related to specific event title
            DeleteInValidPromodes - 
            DeletePromocode - 
            GeneratePromocode - Creates bunch of promocodes depending on Quantity, Title, Expiration date and Target item (here is mostly category of products in catalog)
        */

        public PromocodeController(IPromocodeRepository promocodeRepository, ILogger<PromocodeController> logger)
        {
            _repository = promocodeRepository ?? throw new ArgumentNullException(nameof(promocodeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PromoCode>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PromoCode>>> GetPromoCodes()
        {
            var promocodes = await _repository.GetAllPromocodesAsync();
            return promocodes != null ? Ok(promocodes) : (ActionResult<IEnumerable<PromoCode>>)NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(PromoCode), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<PromoCode>> CreatePromocode([FromBody] PromoCode promocode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                PromoCode toCreateCode = new PromoCode()
                {
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = promocode.Title,
                    Target = promocode.Target,
                    ExpirationDate = promocode.ExpirationDate,
                    Discount = promocode.Discount,
                };

                await _repository.CreateAsync(toCreateCode);

                return StatusCode(201);
            }
            catch
            {
                return BadRequest("Some problems occured during promocode creation");
            }
        }

        [HttpGet("{title}")]
        [ProducesResponseType(typeof(PromoCode), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PromoCode>>> GetPromocodesByTitle(string title)
        {
            var promosByTitle = await _repository.GetPromocodesByTitleAsync(title);
            return promosByTitle != null ? Ok(promosByTitle) : (ActionResult<IEnumerable<PromoCode>>)NotFound($"No promo codes found for event title = {title}");
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePromocode([FromBody] PromoGeneration pc_details)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                List<PromoCode> promoToSave = new List<PromoCode>();

                for (int i = 0; i < pc_details.Quantity; i++)
                {
                    PromoCode pc = new PromoCode()
                    {
                        Code = PromocodeGenerator.GenerateCode(),
                        Title = pc_details.Title,
                        Target = pc_details.Target,
                        ExpirationDate = pc_details.ExpirationDate,
                        Discount = pc_details.Discount
                    };

                    promoToSave.Add(pc);
                }

                await _repository.CreateManyAsync(promoToSave);

                return StatusCode(201);
            }
            catch
            {
                return BadRequest("Some problems occured during the generation of promocodes");
            }
        }
    }
}
