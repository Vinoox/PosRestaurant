using Catalog.Application.Categories.Dtos;
using Catalog.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Categories.Queries.GetAllCategories
{
    // 1. ZAPYTANIE
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
        public required int RestaurantId { get; set; }
    }

    // 2. HANDLER
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICatalogDbContext _dbContext;

        public GetAllCategoriesQueryHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AsNoTracking() // Znacznie przyspiesza pobieranie!
                .Where(c => c.RestaurantId == request.RestaurantId) // KRYTYCZNE: Izolacja danych
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    RestaurantId = c.RestaurantId
                })
                .ToListAsync(cancellationToken);
        }
    }
}