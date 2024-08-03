using MediatR;
using Microsoft.AspNetCore.Mvc;
using TorinoRestaurant.API.Infrastructure.ActionResults;
using TorinoRestaurant.Application.Common.Models;
using TorinoRestaurant.Application.Products.Command;
using TorinoRestaurant.Application.Products.Models;
using TorinoRestaurant.Application.Products.Queries;

namespace TorinoRestaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    // [Authorize]
    public sealed class ProductsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(long id)
        {
            var product = await _mediator.Send(new GetProductDetailQuery(id));
            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ProductEntity>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetProductsQuery request)
        {
            var Products = await _mediator.Send(request);
            return Ok(Products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedResultEnvelope), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromForm] ProductCreateUpdateEntity product)
        {
            var id = await _mediator.Send(new CreateProductCommand(product.Name, product.Description, product.VietnameseDescription, product.CategoryId, product.Price, product.CostPrice, product.IsUseForPrinter, product.Image));
            return CreatedAtAction(nameof(Post), new { id }, new CreatedResultEnvelope(id.ToString()));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(long id, [FromBody] ProductCreateUpdateEntity product)
        {
            await _mediator.Send(new UpdateProductCommand(id, product.Name, product.Description, product.VietnameseDescription, product.CategoryId, product.Price, product.CostPrice, product.IsUseForPrinter, product.IsDeleteImage, product.Image));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(long id)
        {
            await _mediator.Send(new DeleteProductCommand([id]));
            return NoContent();
        }

        [HttpDelete("bulk/{ids}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(List<long> ids)
        {
            await _mediator.Send(new DeleteProductCommand(ids));
            return NoContent();
        }
    }
}