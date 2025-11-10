using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.StaffManagement.Dtos.Commands
{
    public class AddStaffMemberDto
    {
        public required string Email { get; set; }
        public string RoleName { get; set; }
    }
}
