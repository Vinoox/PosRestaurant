using MediatR;

public record GetUserQuery(string UserId) : IRequest<UserDto>;