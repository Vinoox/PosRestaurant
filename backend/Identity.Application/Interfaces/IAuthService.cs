using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.Auth.Commands;
using Identity.Application.Auth.Queries;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task <IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto);
        //Task<string> LoginByPinAsync(LoginByPinDto dto);
    }
}
