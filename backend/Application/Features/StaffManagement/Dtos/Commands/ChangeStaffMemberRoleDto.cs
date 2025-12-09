using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.StaffManagement.Dtos.Commands
{
    public class ChangeStaffMemberRoleDto
    {
        public required string Email { get; set; }
        public required string NewRole { get; set; }
    }
}
