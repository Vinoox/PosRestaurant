using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(string UserId, string OldPassword, string NewPassword) : IRequest<IdentityResult>;


