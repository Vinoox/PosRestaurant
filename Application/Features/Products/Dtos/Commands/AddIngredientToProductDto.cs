using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Features.Products.Dtos.Commands
{
    public class AddIngredientToProductDto
    {
        public int IngredientId { get; set; }
        public decimal Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
