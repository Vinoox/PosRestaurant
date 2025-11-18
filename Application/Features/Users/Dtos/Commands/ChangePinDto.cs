using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Dtos.Commands
{
    public class ChangePinDto
    {
        public required string OldPin { get; set; }
        public required string NewPin { get; set; }
        public required string ConfirmNewPin { get; set; }
    }
}
