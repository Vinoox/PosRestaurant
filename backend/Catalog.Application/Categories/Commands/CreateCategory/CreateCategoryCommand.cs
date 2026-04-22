using MediatR;

namespace Catalog.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public required string Name { get; set; }
        public int RestaurantId { get; set; }
    }
}