using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Dtos
{
    public class ChangePinDto
    {
        public string OldPin { get; set; } = null!;
        public string NewPin { get; set; } = null!;
        public string ConfirmNewPin { get; set; } = null!;
    }
}
