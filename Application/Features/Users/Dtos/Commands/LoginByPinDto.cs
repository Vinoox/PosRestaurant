using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Dtos.Commands
{
    public class LoginByPinDto
    {
        public string Email { get; set; }
        public string Pin { get; set; }
        //public int RestaurantId { get; set; }
    }
}
