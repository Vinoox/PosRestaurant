using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class LoginByPinDto
    {
        public required string Email { get; set; }
        public required string Pin { get; set; }
    }
}
