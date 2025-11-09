using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Restaurants.Dtos.Commands
{
    public class AddStaffMemberDto
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
