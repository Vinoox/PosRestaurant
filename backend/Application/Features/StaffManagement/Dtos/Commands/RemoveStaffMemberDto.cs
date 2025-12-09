using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.StaffManagement.Dtos.Commands
{
    public class RemoveStaffMemberDto
    {
        public required string Email { get; set; }
    }
}
