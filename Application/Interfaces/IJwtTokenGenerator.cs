using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IEnumerable<string> roles);
    }
}