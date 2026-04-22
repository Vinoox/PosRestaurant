using Catalog.Application.Categories.Commands.CreateCategory;
using Catalog.Application.Categories.Queries.GetAllCategories;
using Catalog.Application.Categories.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // Automatycznie: api/v1/categories
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll([FromQuery] int restaurantId)
        {
            // Wersja PRO: RestaurantId docelowo wyciągamy z ClaimsPrincipal (User)
            var result = await _mediator.Send(new GetAllCategoriesQuery { RestaurantId = restaurantId });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await _mediator.Send(command);
            // Zwracamy 201 i ścieżkę do nowego zasobu: api/v1/categories/{id}
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            // Tutaj wywołałbyś GetCategoryByIdQuery
            return Ok();
        }
    }
}