using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return products != null ? Ok(products) : (ActionResult<IEnumerable<Product>>)NoContent();
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _repository.GetProduct(id);
            return product != null ? Ok(product) : (ActionResult<Product>)NotFound();
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var productsByCategory = await _repository.GetProductByCategory(category);
            return productsByCategory != null ? Ok(productsByCategory) : (ActionResult<IEnumerable<Product>>)NotFound($"No products exists for category = {category}");
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Product toCreateProd = new Product()
                {
                    Name = product.Name,
                    Category = product.Category,
                    Summary = product.Summary,
                    Description = product.Description,
                    ImageFile = product.ImageFile,
                    Price = product.Price
                };

                await _repository.Create(toCreateProd);

                return StatusCode(201);
            }
            catch
            {
                return BadRequest("Oops, some problems occured");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product toUpd = new Product()
            {
                Id = value.Id,
                Name = value.Name,
                Category = value.Category,
                Summary = value.Summary,
                Description = value.Description,
                ImageFile = value.ImageFile,
                Price = value.Price
            };

            var res = await _repository.Update(toUpd);

            if (!res)
            {
                return BadRequest("Oops, some problems occured during update");
            }

            return Ok();

        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var delRes = await _repository.Delete(id);

            if (!delRes)
            {
                return BadRequest("Some problems occured during deletion");
            }

            return Ok();
        }
    }
}
