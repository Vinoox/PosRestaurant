using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateAuthenticationToken(User user, IEnumerable<string> globalRoles);
        string GenerateContextualToken(User user, int restaurantId, IEnumerable<string> restaurantRoles);
    }
}