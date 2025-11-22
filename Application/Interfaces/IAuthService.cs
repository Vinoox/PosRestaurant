using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task <IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto);
        Task<string> GenerateContextualTokenAsync(string userId, SelectRestaurantDto dto);
        //Task<string> LoginByPinAsync(LoginByPinDto dto);
    }
}
