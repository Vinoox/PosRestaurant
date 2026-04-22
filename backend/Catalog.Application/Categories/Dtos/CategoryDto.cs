namespace Catalog.Application.Categories.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int RestaurantId { get; set; }
    }
}