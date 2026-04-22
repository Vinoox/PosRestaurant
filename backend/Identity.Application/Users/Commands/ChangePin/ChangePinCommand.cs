using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Users.Commands.ChangePin
{
    public class ChangePinCommand
    {
        public required string OldPin { get; set; }
        public required string NewPin { get; set; }
        public required string ConfirmNewPin { get; set; }
    }
}
