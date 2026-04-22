using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly UserManager<User> _userManager;

    public GetUserQueryHandler(UserManager<User> userManager) => _userManager = userManager;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new NotFoundException("Użytkownik", request.UserId);

        return new UserDto(user.Id, user.Email!, user.FirstName, user.LastName);
    }
}