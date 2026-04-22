using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data; // Zakładając dostęp do DbContext
using MediatR;

namespace Identity.Application.Restaurants.Commands.RegisterRestaurant
{
    public class RegisterRestaurantCommandHandler : IRequestHandler<RegisterRestaurantCommand, Guid>
    {
        private readonly IdentityDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public RegisterRestaurantCommandHandler(IdentityDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Guid> Handle(RegisterRestaurantCommand request, CancellationToken cancellationToken)
        {
            // 1. Logika tworzenia
            var restaurant = Restaurant.Create(request.Dto.Name);
            // Opcjonalnie: ustawienie dodatkowych pól w encji

            // 2. Persystencja
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. (Opcjonalnie) Przypisanie obecnego użytkownika jako właściciela
            // await AssignOwnerToRestaurant(restaurant.Id, _currentUser.UserId);

            return restaurant.Id;
        }
    }
}