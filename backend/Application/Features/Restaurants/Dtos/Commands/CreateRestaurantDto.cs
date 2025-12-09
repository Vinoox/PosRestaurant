using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Restaurants.Dtos.Commands
{
    public class CreateRestaurantDto
    {
        public required string Name { get; set; }
    }
}
