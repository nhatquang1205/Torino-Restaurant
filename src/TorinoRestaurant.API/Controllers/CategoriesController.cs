using MediatR;
using Microsoft.AspNetCore.Mvc;
using TorinoRestaurant.API.Infrastructure.ActionResults;
using TorinoRestaurant.Application.Categories.Command;
using TorinoRestaurant.Application.Categories.Models;
using TorinoRestaurant.Application.Categories.Queries;
using TorinoRestaurant.Application.Common.Models;

namespace TorinoRestaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    // [Authorize]
    public sealed class CategoriesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetail(long id)
        {
            var category = await _mediator.Send(new GetCategoryDetailQuery(id));
            return Ok(category);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<CategoryEntity>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetCategoriesQuery request)
        {
            var categories = await _mediator.Send(request);
            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedResultEnvelope), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromForm] CategoryCreateUpdateEntity category)
        {
            var id = await _mediator.Send(new CreateCategoryCommand(category.Name, category.Description, category.Image));
            return CreatedAtAction(nameof(Get), new { id }, new CreatedResultEnvelope(id.ToString()));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(long id, [FromBody] CategoryCreateUpdateEntity category)
        {
            await _mediator.Send(new UpdateCategoryCommand(id, category.Name, category.Description, category.Image, category.IsDeleteImage));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(long id)
        {
            await _mediator.Send(new DeleteCategoryCommand([id]));
            return NoContent();
        }

        [HttpDelete("bulk/{ids}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(List<long> ids)
        {
            await _mediator.Send(new DeleteCategoryCommand(ids));
            return NoContent();
        }
    }
}